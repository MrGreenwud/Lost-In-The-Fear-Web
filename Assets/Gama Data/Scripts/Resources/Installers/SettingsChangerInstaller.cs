using UnityEngine;
using Zenject;

public class SettingsChangerInstaller : MonoInstaller
{
    [SerializeField] private SettingsChanger m_SettingsChanger;

    public override void InstallBindings()
    {
        Container.Bind<SettingsChanger>().FromInstance(m_SettingsChanger).AsSingle().NonLazy();
    }
}