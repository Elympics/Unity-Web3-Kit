using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
	[SerializeField] private ScenesLoader scenesLoader = null;

	public override void InstallBindings()
	{
		Container.Bind<IScenesLoader>().FromInstance(scenesLoader).AsSingle().NonLazy();
		Container.Bind<IDefaultButtonSoundsProvider>().To<DefaultButtonSoundsProvider>().AsSingle().NonLazy();
		/////////////////////		
		Container.Bind<IWalletAPI>().To<
#if UNITY_EDITOR
			UnityEditorWalletAPI
#else
			MetaMaskAPI
#endif
			>().AsSingle().NonLazy();
		//////////////////////
		Container.Bind<IOrbiesSmartContractAPI>().To<
#if UNITY_EDITOR
			UnityEditorSmartContract
#else
			EthereumABIIntegration
#endif
			>().AsSingle().NonLazy();
		///////////////////////
		Container.Bind<ITokenAPI>().To<
#if UNITY_EDITOR
			UnityEditorTokenConnect
#else
			TokenAPI
#endif
		 >().AsSingle().NonLazy();
	}
}