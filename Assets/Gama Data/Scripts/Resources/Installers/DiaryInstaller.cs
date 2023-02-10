using UnityEngine;
using Zenject;

public class DiaryInstaller : MonoInstaller
{
    [SerializeField] private Diary diary;

    public override void InstallBindings()
    {
        Container.Bind<Diary>().FromInstance(diary).AsSingle().NonLazy();
    }
}