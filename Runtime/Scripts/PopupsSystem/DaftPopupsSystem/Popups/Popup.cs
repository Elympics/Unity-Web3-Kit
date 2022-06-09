using System;
using UnityEngine;

namespace DaftPopups
{
	public abstract class Popup : MonoBehaviour
	{
		[SerializeField] protected bool canBeClosedByFade = true;

		public event Action OnClose;
		public bool CanBeClosedByFade => canBeClosedByFade;

		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
			OnClose?.Invoke();
			ClearActions();
		}

		public virtual void ClearActions()
		{
			OnClose = null;
		}
	}
}