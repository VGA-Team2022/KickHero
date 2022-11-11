using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InGameDataInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISceneData>()
    .To<InGameScene>()
    .FromNewComponentOnNewGameObject()
    .AsSingle()
    .NonLazy();
    }
}
