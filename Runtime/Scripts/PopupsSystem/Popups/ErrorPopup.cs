using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaftPopups;
using DaftUI;
using TMPro;

public class ErrorPopup : PopupInfo
{
	[Header("References:")]
	[SerializeField] private TextMeshProUGUI errorCodeText = null;

	[Header("Error code prefix:")]
	[SerializeField] private string errorCodePrefix = "Error code: ";

	public override void ApplyData(InfoPopupData data)
	{
		base.ApplyData(data);

		if (data is ErrorPopupData errorPopupData)
		{
			errorCodeText.gameObject.SetActive(!errorPopupData.hideErrorCode);
			errorCodeText.text = errorCodePrefix + errorPopupData.errorCode;
		}	
	}
}
