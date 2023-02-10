using UnityEngine;
using Zenject;

public class PlayerControllerInstaller : MonoInstaller
{
    [SerializeField] private PlayerController playerController;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromInstance(playerController).AsSingle().NonLazy();
    }
}