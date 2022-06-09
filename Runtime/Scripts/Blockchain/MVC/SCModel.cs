using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UniRx;
using UnityEngine;

public class SCModel : IModel
{
	public ReactiveProperty<string> NickName => nickName;
	public ReactiveProperty<string> WalletAddress => walletAddress;
	public ReactiveProperty<float> TokenBalance => tokenBalance;
	public ReactiveProperty<float> PayoutBalance => payoutBalance;
	public ReactiveProperty<int> BetValue => betValue;
	public ReactiveProperty<string> BettingHash => bettingHash;
	public ReactiveProperty<float> TokenAllowed => tokenAllowed;

	private ReactiveProperty<string> nickName = new ReactiveProperty<string>("------");
	private ReactiveProperty<string> walletAddress = new ReactiveProperty<string>("-------");
	private ReactiveProperty<float> tokenBalance = new ReactiveProperty<float>(0);
	private ReactiveProperty<float> payoutBalance = new ReactiveProperty<float>(0);
	private ReactiveProperty<int> betValue = new ReactiveProperty<int>(0);
	private ReactiveProperty<string> bettingHash = new ReactiveProperty<string>("-------");
	private ReactiveProperty<float> tokenAllowed = new ReactiveProperty<float>(0);

	public void HandleGuestName(string guestName) => NickName.Value = guestName;
	public void HandleNicknameSet(string nickname) => NickName.Value = nickname;
	public void HandleAllowed(string tokenAllowedAmount) => tokenAllowed.Value = float.Parse(tokenAllowedAmount, CultureInfo.InvariantCulture);
	public void HandleEnterWithBet(string betHash) => bettingHash.Value = betHash;
	public void HandleSetBetValue(string betValue) => this.betValue.Value = int.Parse(betValue, CultureInfo.InvariantCulture);
	public void HandleGetPayout(string payout) => payoutBalance.Value = float.Parse(payout, CultureInfo.InvariantCulture);
	public void HandleWalletBalance(string tokenBalance) => this.tokenBalance.Value = float.Parse(tokenBalance, CultureInfo.InvariantCulture);
	public void HandleWalletAddress(string address) => walletAddress.Value = (address);
	public void HandleGetNickname(string nickname) => NickName.Value = nickname;

}
