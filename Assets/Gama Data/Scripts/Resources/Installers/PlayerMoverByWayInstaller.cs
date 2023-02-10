using UnityEngine;
using Zenject;

public class PlayerMoverByWayInstaller : MonoInstaller
{
    [SerializeField] private PlayerMoverByWay playerMoverByWay;

    public override void InstallBindings()
    {
        Container.Bind<PlayerMoverByWay>().FromInstance(playerMoverByWay).AsSingle().NonLazy();
    }
}