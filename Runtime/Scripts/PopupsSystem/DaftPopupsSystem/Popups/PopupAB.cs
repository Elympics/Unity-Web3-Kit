using DaftUI;
using System;
using UnityEngine;

namespace DaftPopups
{
	public class PopupAB : StandardPopup<ABPopupData>
	{
		[SerializeField] private DaftButton buttonA = null;
		[SerializeField] private DaftButton buttonB = null;

		public event Action OnA;
		public event Action OnB;

		public void OnChoseA()
		{
			OnA?.Invoke();
		}

		public void OnChoseB()
		{
			OnB?.Invoke();
		}

		public override void ClearActions()
		{
			base.ClearActions();
			OnA = null;
			OnB = null;
		}

		public override void ApplyData(ABPopupData data)
		{
			base.ApplyData(data);
			UpdateButton(buttonA, data.buttonAText, data.buttonAStyle);
			UpdateButton(buttonB, data.buttonBText, data.buttonBStyle);
		}

		private void UpdateButton(DaftButton buttonA, string buttonAText, ButtonStyle buttonAStyle)
		{
			buttonA.Text = buttonAText;
			buttonA.ApplyStyle(buttonAStyle);
		}
	}
}
