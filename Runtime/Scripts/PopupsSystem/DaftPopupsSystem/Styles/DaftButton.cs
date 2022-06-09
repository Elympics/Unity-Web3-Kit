using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftUI
{
	[RequireComponent(typeof(Button))]
	public class DaftButton : MonoBehaviour
	{
		[SerializeField] private Button button = null;
		[SerializeField] private TextMeshProUGUI textContent = null;

		public string Text { set => textContent.text = value; get => textContent.text; }

		public void ApplyStyle(ButtonStyle style)
		{
			button.image.sprite = style.sprite;
		}
	}
}