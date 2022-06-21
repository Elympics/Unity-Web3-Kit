using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using Zenject;
using DG.Tweening;

public class CoinsUpdater : MonoBehaviour
{
	[Header("Token exchange website url")]
	[SerializeField] private string url = "https://app.sushi.com/swap?tokens=MATIC&tokens=0x8f3Cf7ad23Cd3CaDbD9735AFf958023239c6A063&chainId=137";

	[Header("Scene References")]
	[SerializeField] private TextMeshProUGUI coinsBalance = null;
	[SerializeField] private float refreshingCoinStatusInSeconds = 3;

	private SCController controller;
	private SmartContractConfig smartContractConfig;

	[Inject]
	private void Inject(SCController controller, SmartContractConfig smartContractConfig)
	{
		this.controller = controller;
		this.smartContractConfig = smartContractConfig;
	}

	private void Start()
	{
		controller.Model.TokenBalance.Subscribe(tokenBalance =>
		{
			coinsBalance.text = controller.Model.TokenBalance.ToString();
			coinsBalance.transform.DOShakeScale(0.1f, 0.2f);
		});

		if (smartContractConfig.useSmartContract)
			StartCoroutine(AutomaticRefresherWalletBalance());
	}

	public void OpenExternalTokenWebsite()
	{
		Application.OpenURL(url);
	}

	private IEnumerator AutomaticRefresherWalletBalance()
	{
		var wait = new WaitForSeconds(refreshingCoinStatusInSeconds);
		while (true)
		{
			yield return wait;
			yield return controller.GetTokenBalance(null);
		}
	}
}
