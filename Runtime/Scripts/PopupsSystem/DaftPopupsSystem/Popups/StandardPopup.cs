using TMPro;
using UnityEngine;

namespace DaftPopups
{
	public abstract class StandardPopup<TData> : PopupWithData<TData> where TData : StandardPopupData
	{
		[SerializeField] protected TextMeshProUGUI header = null;
		[SerializeField] protected TextMeshProUGUI content = null;

		public override void ApplyData(TData data)
		{
			header.text = data.header;
			content.text = data.content;
		}
	}
}