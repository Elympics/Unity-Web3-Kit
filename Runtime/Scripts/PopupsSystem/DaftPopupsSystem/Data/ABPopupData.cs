using DaftUI;
using UnityEngine;

namespace DaftPopups
{
	[CreateAssetMenu(fileName = "ABPopupData", menuName = MenuPaths.CreateAssetPrefix + "ABPopup", order = CreateAssetMenuOrder)]
	public class ABPopupData : StandardPopupData
	{
		public const int CreateAssetMenuOrder = InfoPopupData.CreateAssetMenuOrder + 1;

		public string buttonAText;
		public ButtonStyle buttonAStyle;
		public string buttonBText;
		public ButtonStyle buttonBStyle;
	}
}