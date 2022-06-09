using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DaftPopups
{
	public class PopupsManager : MonoBehaviour
	{
		[SerializeField] private List<Popup> popupPrefabs = null;
		[SerializeField] private GameObject fade = null;
		[SerializeField] private Transform popupsContainer = null;

		private Dictionary<Type, Popup> popups = new Dictionary<Type, Popup>();
		private LinkedList<Popup> activePopupsList = new LinkedList<Popup>();
		private DiContainer diContainer = null;

		[Inject]
		private void Init(DiContainer diContainer)
		{
			this.diContainer = diContainer;

			foreach (var popup in popupPrefabs)
			{
				if (popup != null)
					popups.Add(popup.GetType(), popup);
			}
		}

		public PopupAB ShowPopupAB(Action onA, Action onB, Action onClose, ABPopupData data)
			=> ShowPopupAB<PopupAB>(onA, onB, onClose, data);

		public TPopup ShowPopupAB<TPopup>(Action onA, Action onB, Action onClose, ABPopupData data) where TPopup : PopupAB
		{
			var popup = ShowPopup<TPopup>();

			popup.OnA += onA;
			popup.OnB += onB;
			popup.OnClose += onClose;

			popup.ApplyData(data);

			return popup;
		}

		public PopupInfo ShowPopupInfo(Action onClose, InfoPopupData data)
			=> ShowPopupInfo<PopupInfo>(onClose, data);

		public TPopup ShowPopupInfo<TPopup>(Action onClose, InfoPopupData data) where TPopup : PopupInfo
		{
			var popupInfo = ShowPopup<TPopup>();

			popupInfo.OnClose += onClose;

			popupInfo.ApplyData(data);

			return popupInfo;
		}

		public TPopup ShowPopup<TPopup>() where TPopup : Popup
		{
			if (!popups.ContainsKey(typeof(TPopup)))
				return null;

			return ShowPopup((TPopup)popups[typeof(TPopup)]);
		}

		public TPopup ShowPopup<TPopup>(TPopup popupPrefab) where TPopup : Popup
		{
			var chosenPopup = diContainer.InstantiatePrefabForComponent<TPopup>(popupPrefab, popupsContainer);
			chosenPopup.Show();

			var node = activePopupsList.AddLast(chosenPopup);
			chosenPopup.OnClose += () => OnPopupClosed(node);

			UpdateFadeVisibility();

			return chosenPopup;
		}

		private void OnPopupClosed(LinkedListNode<Popup> node)
		{
			if (node.Value != null)
				Destroy(node.Value.gameObject);

			activePopupsList.Remove(node);

			UpdateFadeVisibility();
		}

		public void OnFadePressed()
		{
			Popup activePopup = activePopupsList.Last.Value;

			if (activePopup.CanBeClosedByFade)
				activePopup.Hide();
		}

		private void UpdateFadeVisibility()
		{
			if (fade != null)
				fade.SetActive(activePopupsList.Count != 0);
		}
	}
}


