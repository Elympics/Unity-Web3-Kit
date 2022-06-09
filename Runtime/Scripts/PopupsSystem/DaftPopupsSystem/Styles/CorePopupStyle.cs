using UnityEngine;

namespace DaftPopups
{
	[CreateAssetMenu(fileName = "CorePopupStyle", menuName = MenuPaths.CreateAssetPrefix + MenuPaths.CreateStyleAssetPrefix + "CorePopupStyle", order = CreateAssetMenuOrder)]
	public class CorePopupStyle : ScriptableObject
	{
		public const int CreateAssetMenuOrder = MenuPaths.FirstOrder;
	}
}