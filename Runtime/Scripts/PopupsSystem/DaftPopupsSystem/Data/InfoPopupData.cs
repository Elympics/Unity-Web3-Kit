using DaftUI;
using UnityEngine;

namespace DaftPopups
{
	[CreateAssetMenu(fileName = "InfoPopupData", menuName = MenuPaths.CreateAssetPrefix + "InfoPopup", order = CreateAssetMenuOrder)]
	public class InfoPopupData : StandardPopupData
	{
		public const int CreateAssetMenuOrder = MenuPaths.FirstOrder + 1000;

		public string buttonText;
		public ButtonStyle buttonStyle;
	}
}
