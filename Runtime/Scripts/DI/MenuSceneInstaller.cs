using Elympics;
using Zenject;
using ElympicsRoomsAPI;

public class MenuSceneInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<GameMetaData>().AsSingle().NonLazy();
		Container.Bind<ElympicsRoomsAPIController>().FromComponentInHierarchy().AsSingle().NonLazy();
		Container.Bind<MainMenuController>().FromComponentInHierarchy().AsSingle().NonLazy();
		Container.Bind<TicketPriceController>().FromComponentInHierarchy().AsSingle().NonLazy();

		if (ElympicsLobbyClient.Instance != null)
			Container.Bind<ElympicsLobbyClient>().FromInstance(ElympicsLobbyClient.Instance).AsSingle().NonLazy();
		else
			Container.Bind<ElympicsLobbyClient>().FromComponentInHierarchy().AsSingle().NonLazy();
	}
}