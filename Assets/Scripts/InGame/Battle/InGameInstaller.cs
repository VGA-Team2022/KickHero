using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InGameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IReceivableGameData>()
            .To<InGameCycle>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();
    }
}
