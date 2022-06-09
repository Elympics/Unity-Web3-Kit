using UnityEngine;
using Zenject;

namespace DaftPopups
{
	public class DaftPopupsSystemInstaller : MonoInstaller
	{
		[SerializeField] private PopupsManager popupsManagerPrefab = null;

		public override void InstallBindings()
		{
			Container.Bind<PopupsManager>().FromComponentInNewPrefab(popupsManagerPrefab).AsSingle().NonLazy();
		}
	}
}