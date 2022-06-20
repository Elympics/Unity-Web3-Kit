using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ElympicsRoomsAPI
{
	public class ElympicsRoomsAPIController : MonoBehaviour
	{
		[Header("References:")]
		[SerializeField] private ElympicsLobbyClient elympicsLobbyClient = null;

		[Header("Settings:")]
		[SerializeField] private string uri = "https://api.clashoforbs.daftmobile.local/api/v1";

		public static ElympicsRoomsAPIController Instance { get; private set; }

		private string jwtToken = null;
		private bool isAuthenticated = false;
		private bool hasWalletAddress = false;

		private IWalletAPI walletApi = null;
		private string walletAddress = null;
		private ApiCallHandler getWalletAddressHandler;
		private GameMetaData gameMetaData = null;

		public bool IsReady => isAuthenticated && hasWalletAddress;
		public event Action<bool> IsReadyChanged = null;

		private void Awake()
		{
			Initialize();
			elympicsLobbyClient.Authenticated += UpdateJwtToken;
		}

		private void Start()
		{
			StartCoroutine(walletApi.GetWalletAddress(getWalletAddressHandler));
		}

		private void Initialize()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(this.gameObject);
				return;
			}
		}

		private void UpdateJwtToken(bool success, string userId, string jwtToken, string error)
		{
			if (success)
			{
				isAuthenticated = success;
				this.jwtToken = jwtToken;

				IsReadyChanged?.Invoke(IsReady);
			}
		}

		[Inject]
		private void Inject(IWalletAPI walletApi, GameMetaData gameMetaData, ElympicsRoomAPIConfig config)
		{
			this.walletApi = walletApi;
			this.gameMetaData = gameMetaData;
			ValidateUri(config);
			getWalletAddressHandler = new ApiCallHandler(HandleWalletAddress, HandleError);
		}

		private void ValidateUri(ElympicsRoomAPIConfig config)
		{
			if (config != null)
			{
				if (string.IsNullOrEmpty(config.Uri) || !Uri.IsWellFormedUriString(config.Uri, UriKind.Absolute))
					Debug.LogError("[Web3Kit:RoomAPI] Uri provided in the Room API Config is invalid! Config is at Resources/" + ElympicsRoomAPIConfig.PATH_IN_RESOURCES);
				else
					uri = config.Uri;
			}
			else
			{
				Debug.LogError("[Web3Kit:RoomAPI] Config not found! Did you run first time setup?");
			}
		}

		//Debug tests only
		private void OnApplicationQuit()
		{
			if (gameMetaData.RoomId != null && gameMetaData.RoomId != "")
				CloseRoomIfEmpty(gameMetaData.RoomId);
		}

		private void HandleWalletAddress(string address)
		{
			hasWalletAddress = true;
			this.walletAddress = address;
			IsReadyChanged?.Invoke(IsReady);
		}

		private void HandleError(string error)
		{
			Debug.LogError(error);
		}

		public void UpdatePlayerNickname(string nickname)
		{
			string fullUri = $"{uri}/players/me/";

			byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"nickname\": \"" + nickname + "\",\"address\": \"" + walletAddress + "\"}");
			Debug.Log("Trying to send: " + "{\"nickname\": \"" + nickname + "\",\"address\": \"" + walletAddress + "\"}");

			SendWebRequest(bodyRaw, fullUri, "PATCH", OnResponseReceived);

			void OnResponseReceived(string response)
			{
				Debug.Log("Player nickname updated! " + response);
			}
		}

		public void CreateChallengeFriendRoom(int bet, Action<ElympicsRoomsAPIResponses.Room> ProcessReceivedRoom)
		{
			string fullUri = $"{uri}/rooms/";

			byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"bet\": {\"amount\": " + bet + ",\"address\": \"" + walletAddress + "\"}}");

			SendWebRequest(bodyRaw, fullUri, "POST", OnResponseReceived);

			void OnResponseReceived(string response)
			{
				ElympicsRoomsAPIResponses.Room room = response != null ? JsonUtility.FromJson<ElympicsRoomsAPIResponses.Room>(response) : null;

				ProcessReceivedRoom(room);
			}
		}

		public void GetCurrentPlayer(Action<ElympicsRoomsAPIResponses.ElympicsPlayer> ProcessReceivedPlayer)
		{
			string fullUri = $"{uri}/players/me/";

			SendWebRequest(null, fullUri, "GET", OnResponseReceived);

			void OnResponseReceived(string response)
			{
				ElympicsRoomsAPIResponses.ElympicsPlayer player = response != null ? JsonUtility.FromJson<ElympicsRoomsAPIResponses.ElympicsPlayer>(response) : null;

				ProcessReceivedPlayer(player);
			}
		}

		public void GetRoomInfo(string roomId, Action<ElympicsRoomsAPIResponses.Room> ProcessReceivedRoom)
		{
			string fullUri = $"{uri}/rooms/{roomId}/";

			SendWebRequest(null, fullUri, "GET", OnResponseReceived, new (string header, string value)[] { ("room_key", roomId) });

			void OnResponseReceived(string response)
			{
				ElympicsRoomsAPIResponses.Room room = response != null ? JsonUtility.FromJson<ElympicsRoomsAPIResponses.Room>(response) : null;

				ProcessReceivedRoom(room);
			}
		}

		public void JoinRoom(string roomId, Action<ElympicsRoomsAPIResponses.Room> ProcessReceivedRoom)
		{
			string fullUri = $"{uri}/rooms/{roomId}/join";

			SendWebRequest(null, fullUri, "POST", OnResponseReceived, new (string header, string value)[] { ("room_key", roomId) });

			void OnResponseReceived(string response)
			{
				ElympicsRoomsAPIResponses.Room room = response != null ? JsonUtility.FromJson<ElympicsRoomsAPIResponses.Room>(response) : null;

				ProcessReceivedRoom(room);
			}
		}

		public void LeaveRoom(string roomId)
		{
			string fullUri = $"{uri}/rooms/{roomId}/leave";

			SendWebRequest(null, fullUri, "POST", OnResponseReceived, new (string header, string value)[] { ("room_key", roomId) });

			void OnResponseReceived(string response)
			{
				Debug.Log("Player left the room, response: ");
				Debug.Log(response);

				CloseRoomIfEmpty(roomId);
			}
		}

		private void CloseRoomIfEmpty(string roomId)
		{
			GetRoomInfo(roomId, OnRoomInfoReceived);

			void OnRoomInfoReceived(ElympicsRoomsAPIResponses.Room room)
			{
				if (room == null)
					return;

				Debug.Log("Room status: " + room.state);
				Debug.Log("Number of players in room: " + room.players.Length);

				if (room != null && room.players.Length == 0 && room.state == ElympicsRoomsAPIResponses.RoomState.open)
				{
					Debug.Log("Closing room...");

					CloseRoom(roomId);
				}
			}
		}

		public void CloseRoom(string roomId)
		{
			string fullUri = $"{uri}/rooms/{roomId}/close";

			Debug.Log("Full uri for closing room: " + fullUri);
			SendWebRequest(null, fullUri, "POST", OnResponseReceived, new (string header, string value)[] { ("room_key", roomId) });

			void OnResponseReceived(string response)
			{
				ElympicsRoomsAPIResponses.Room room = response != null ? JsonUtility.FromJson<ElympicsRoomsAPIResponses.Room>(response) : null;

				if (room != null && room.state == ElympicsRoomsAPIResponses.RoomState.closed)
					Debug.Log($"Room {roomId} closed!");
				else
					Debug.LogError($"Something went wrong while closing room {roomId}");
			}
		}

		private void SendWebRequest(byte[] body, string uri, string method, Action<string> callback, (string header, string value)[] additionalHeaders = null)
		{
			UnityWebRequest www = new UnityWebRequest(uri, method);

			if (additionalHeaders != null)
				foreach ((string header, string value) additionalHeader in additionalHeaders)
					www.SetRequestHeader(additionalHeader.header, additionalHeader.value);

			www.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);

			StartCoroutine(ProcessWebRequest(www, callback));
		}

		private IEnumerator ProcessWebRequest(UnityWebRequest webRequest, Action<string> ResponseReceived)
		{
			webRequest.SetRequestHeader("accept", "application/json");
			webRequest.SetRequestHeader("Authorization", "Bearer " + jwtToken);
			webRequest.SetRequestHeader("Content-Type", "application/json");

			webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

			yield return webRequest.SendWebRequest();

			if (webRequest.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(webRequest.error);
				Debug.LogError(webRequest.downloadHandler.text);
				Debug.LogError(webRequest.responseCode);
				ResponseReceived(null);
			}
			else
			{
				ResponseReceived(webRequest.downloadHandler.text);
			}
		}
	}

}