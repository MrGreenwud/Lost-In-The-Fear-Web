using UnityEngine;
using Zenject;

public class InputHundlerInstaller : MonoInstaller
{
    [SerializeField] private InputHandler inputHandler;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();
    }
}