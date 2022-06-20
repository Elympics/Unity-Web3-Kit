using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
	public ScenesLoader scenesLoader = null;

	public override void InstallBindings()
	{
		Container.Bind<GameMetaData>().AsSingle().NonLazy();

		Container.Bind<IScenesLoader>().FromInstance(scenesLoader).AsSingle().NonLazy();
		Container.Bind<IDefaultButtonSoundsProvider>().To<DefaultButtonSoundsProvider>().AsSingle().NonLazy();

		var roomConfig = Resources.Load<ElympicsRoomAPIConfig>(ElympicsRoomAPIConfig.PATH_IN_RESOURCES);
		if (roomConfig == null)
			Debug.LogError("[Web3Kit:RoomAPI] Config not found! Did you run the first time setup?");
		Container.Bind<ElympicsRoomAPIConfig>().FromInstance(roomConfig).AsSingle().NonLazy();

		var scConfig = Resources.Load<SmartContractConfig>(SmartContractConfig.PATH_IN_RESOURCES);
		if (scConfig == null)
			Debug.LogError("[Web3Kit:SmartContract] Config not found! Did you run the first time setup?");
		Container.Bind<SmartContractConfig>().FromInstance(scConfig).AsSingle().NonLazy();

		Container.Bind<IWalletAPI>().To<
#if UNITY_EDITOR
			UnityEditorWalletAPI
#else
			MetaMaskAPI
#endif
			>().AsSingle().NonLazy();

		Container.Bind<IOrbiesSmartContractAPI>().To<
#if UNITY_EDITOR
			UnityEditorSmartContract
#else
			EthereumABIIntegration
#endif
			>().AsSingle().NonLazy();

		Container.Bind<ITokenAPI>().To<
#if UNITY_EDITOR
			UnityEditorTokenConnect
#else
			TokenAPI
#endif
		 >().AsSingle().NonLazy();
	}
}