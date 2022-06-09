using UnityEngine;

namespace DaftUI
{
	[CreateAssetMenu(fileName = "ButtonStyle", menuName = MenuPaths.CreateAssetPrefix + MenuPaths.CreateStyleAssetPrefix +  "ButtonStyle", order = CreateAssetMenuOrder)]
	public class ButtonStyle : ScriptableObject
	{
		public const int CreateAssetMenuOrder = 2;

		public Sprite sprite;
	}
}