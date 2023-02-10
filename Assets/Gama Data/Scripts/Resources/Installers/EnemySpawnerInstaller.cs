using UnityEngine;
using Zenject;

public class EnemySpawnerInstaller : MonoInstaller
{
    [SerializeField] private EnemySpawner m_EnemySpawner;

    public override void InstallBindings()
    {
        Container.Bind<EnemySpawner>().FromInstance(m_EnemySpawner).AsSingle().NonLazy();
    }
}
