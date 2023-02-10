using UnityEngine;
using Zenject;

public class HeadRotatorInstaller : MonoInstaller
{
    [SerializeField] private HeadRoatator headRoatator;

    public override void InstallBindings()
    {
        Container.Bind<HeadRoatator>().FromInstance(headRoatator).AsSingle().NonLazy();
    }
}