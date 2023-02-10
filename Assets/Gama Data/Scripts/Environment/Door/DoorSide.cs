using System;
using UnityEngine;
using Side = Door.Side;

[RequireComponent(typeof(BoxCollider))]
public class DoorSide : MonoBehaviour
{
    [SerializeField] private Side m_Side;

    public Action OnEnter;

    public Side GetSide() => m_Side;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController playerController))
            OnEnter?.Invoke();
    }
}
