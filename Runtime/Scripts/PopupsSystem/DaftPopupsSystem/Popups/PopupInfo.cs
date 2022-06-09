using DaftUI;
using TMPro;
using UnityEngine;

namespace DaftPopups
{
	public class PopupInfo : StandardPopup<InfoPopupData>
	{
		[SerializeField] private DaftButton daftButton;

		public override void ApplyData(InfoPopupData data)
		{
			base.ApplyData(data);
			daftButton.ApplyStyle(data.buttonStyle);
			daftButton.Text = data.buttonText;
		}
	}
}
