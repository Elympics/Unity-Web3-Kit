using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MVCInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<SCController>().FromComponentOn(this.gameObject).AsSingle().NonLazy();
	}
}
