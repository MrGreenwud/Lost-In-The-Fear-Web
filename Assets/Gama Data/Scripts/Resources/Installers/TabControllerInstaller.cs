using UnityEngine;
using Zenject;

public class TabControllerInstaller : MonoInstaller
{
    [SerializeField] private TabController tabController;

    public override void InstallBindings()
    {
        Container.Bind<TabController>().FromInstance(tabController).AsSingle().NonLazy();
    }
}