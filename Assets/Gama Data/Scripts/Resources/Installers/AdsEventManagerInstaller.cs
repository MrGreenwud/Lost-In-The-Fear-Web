using UnityEngine;
using Zenject;

public class AdsEventManagerInstaller : MonoInstaller
{
    [SerializeField] private AdsEventManager m_AdsEventManager;

    public override void InstallBindings()
    {
        Container.Bind<AdsEventManager>().FromInstance(m_AdsEventManager).AsSingle().NonLazy();
    }
}