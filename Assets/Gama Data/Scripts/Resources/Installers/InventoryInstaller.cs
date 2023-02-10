using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private Inventory inventoryController;

    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromInstance(inventoryController).AsSingle().NonLazy();
    }
}