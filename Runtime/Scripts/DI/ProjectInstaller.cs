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

		if (scConfig.useSmartContract
#if UNITY_EDITOR
			&& false
#endif
			)
		{
			Container.Bind<IWalletAPI>().To<MetaMaskAPI>().AsSingle().NonLazy();
			Container.Bind<IOrbiesSmartContractAPI>().To<EthereumABIIntegration>().AsSingle().NonLazy();
			Container.Bind<ITokenAPI>().To<TokenAPI>().AsSingle().NonLazy();
		}
		else
		{
			Container.Bind<IWalletAPI>().To<UnityEditorWalletAPI>().AsSingle().NonLazy();
			Container.Bind<IOrbiesSmartContractAPI>().To<UnityEditorSmartContract>().AsSingle().NonLazy();
			Container.Bind<ITokenAPI>().To<UnityEditorTokenConnect>().AsSingle().NonLazy();
		}
	}
}