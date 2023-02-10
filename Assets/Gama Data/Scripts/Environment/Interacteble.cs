using System;
using UnityEngine;

public class Interacteble : MonoBehaviour
{
    public Action OnInteract;

    protected int p_IteractCount;
    protected bool p_IsInteracteble = true;

    public virtual void Interact(Slot slot) 
    {
        p_IteractCount++;
        OnInteract?.Invoke(); 
    }
    
    public virtual void Interact() 
    {
        p_IteractCount++;
        OnInteract?.Invoke(); 
    }
}
