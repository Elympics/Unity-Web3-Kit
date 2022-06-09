using System;
using UniRx;

public interface IModel
{
	public ReactiveProperty<string> NickName { get; }
	public ReactiveProperty<string> WalletAddress { get; }
	public ReactiveProperty<float> TokenBalance { get; }
	public ReactiveProperty<float> PayoutBalance { get; }
	public ReactiveProperty<int> BetValue { get; }
	public ReactiveProperty<string> BettingHash { get; }
	public ReactiveProperty<float> TokenAllowed { get; }
}