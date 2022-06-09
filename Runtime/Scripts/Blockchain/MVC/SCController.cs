using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DaftPopups;
using UnityEngine;
using Zenject;
using TMPro;
using UniRx;

public class SCController : MonoBehaviour
{
	[SerializeField] private ErrorPopupData smartContractErrorData = null;

	public IModel Model => model;

	private SCModel model;
	private IOrbiesSmartContractAPI smartContractAPI;
	private IWalletAPI walletAPI;
	private ITokenAPI tokenAPI;
	private IScenesLoader scenesLoader;
	private PopupsManager popupsManager;

	IModel CreateModel()
	{
		model = new SCModel();
		return model;
	}

	[Inject]
	public void Inject(IOrbiesSmartContractAPI smartContractAPI, ITokenAPI tokenAPI, IWalletAPI walletAPI, PopupsManager popupsManager, IScenesLoader scenesLoader)
	{
		this.popupsManager = popupsManager;
		this.scenesLoader = scenesLoader;

		CreateModel();

		this.smartContractAPI = smartContractAPI;
		this.walletAPI = walletAPI;
		this.tokenAPI = tokenAPI;
	}

	private void HandleError(string error)
	{
		popupsManager.ShowPopupInfo<ErrorPopup>(scenesLoader.LoadMainMenu, smartContractErrorData);
		Debug.Log(error);
	}

	public IEnumerator SetBetValue(int betValue)
	{
		model.BetValue.Value = betValue;
		yield return GetPayout(model.BetValue.Value, null);
	}

	public void UseGuestName()
	{
		var guestName = $"Player#{UnityEngine.Random.Range(1000, 9999)}";
		model.HandleGuestName(guestName);
	}

	public IEnumerator CheckConnection(ApiCallHandler.ErrorCallback errorHandler)
	{
		var apiCallHandler = new ApiCallHandler(null, HandleError + errorHandler);
		yield return StartCoroutine(smartContractAPI.CheckNetworkConnection(apiCallHandler));
	}

	public IEnumerator ConnectToMetamask(ApiCallHandler.ErrorCallback errorHandler)
	{
		var apiCallHandler = new ApiCallHandler(null, HandleError + errorHandler);
		yield return StartCoroutine(walletAPI.Connect(apiCallHandler));
	}

	public IEnumerator GetWalletAddress(ApiCallHandler.ErrorCallback errorHandler)
	{
		var walletAddressHandel = new ApiCallHandler(model.HandleWalletAddress, HandleError + errorHandler);
		yield return StartCoroutine(walletAPI.GetWalletAddress(walletAddressHandel));
	}

	public IEnumerator GetNickname(ApiCallHandler.ErrorCallback errorHandler)
	{
		var getNicknameHandler = new ApiCallHandler(model.HandleGetNickname, HandleError + errorHandler);
		yield return StartCoroutine(smartContractAPI.GetNickname(getNicknameHandler));
	}

	public IEnumerator SetNickname(string nickname, ApiCallHandler.ErrorCallback errorHandler)
	{
		var setNicknameHandler = new ApiCallHandler(model.HandleNicknameSet, HandleError + errorHandler);
		yield return StartCoroutine(smartContractAPI.SetNickname(nickname, setNicknameHandler));
	}

	public IEnumerator GetTokenBalance(ApiCallHandler.ErrorCallback errorHandler)
	{
		var walletBalanceHandler = new ApiCallHandler(model.HandleWalletBalance, HandleError + errorHandler);
		yield return StartCoroutine(tokenAPI.TokenBalance(model.WalletAddress.Value, walletBalanceHandler));
	}

	public IEnumerator GetPayout(int betAmount, ApiCallHandler.ErrorCallback errorHandler)
	{
		var getPayoutHandler = new ApiCallHandler(model.HandleGetPayout, HandleError + errorHandler);
		yield return StartCoroutine(smartContractAPI.GetPayout(betAmount.ToString(), getPayoutHandler));
	}

	public IEnumerator EnterWithBet(int betAmount, ApiCallHandler.ErrorCallback errorHandler)
	{
		var enterWithBetHandler = new ApiCallHandler(model.HandleEnterWithBet, HandleError + errorHandler);
		yield return StartCoroutine(smartContractAPI.EnterWithBet(betAmount.ToString(), enterWithBetHandler));
	}

	public IEnumerator TokenAllowedAmount(ApiCallHandler.ErrorCallback errorHandler)
	{
		var isAllowedHandler = new ApiCallHandler(model.HandleAllowed, HandleError + errorHandler);
		yield return StartCoroutine(tokenAPI.TokenABIIsAllowed(model.WalletAddress.Value, isAllowedHandler));
	}

	public IEnumerator TokenApprove(ApiCallHandler.ErrorCallback errorHandler)
	{
		var isApproveHandler = new ApiCallHandler(null, HandleError + errorHandler);
		yield return StartCoroutine(tokenAPI.TokenABITokenAproval(isApproveHandler));
	}
}
