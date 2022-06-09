using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaftPopups;
using DaftUI;

[CreateAssetMenu(fileName = "ErrorPopupData", menuName = MenuPaths.CreateAssetPrefix + "ErrorPopup", order = CreateAssetMenuOrder)]
public class ErrorPopupData : InfoPopupData
{
	public bool hideErrorCode = true;
	public string errorCode;
}
