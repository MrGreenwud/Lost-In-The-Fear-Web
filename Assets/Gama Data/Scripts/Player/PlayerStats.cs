using System;

public class PlayerStats
{
    private readonly PlayerController m_PlayerController;
    public readonly float MaxHelth;
    public readonly float MaxStamina;

    public float CurrentHelth { get; private set; }
    public float CurrentStamina { get; private set; }

    public Action OnChangeHelth;
    public Action OnChangeStamina;

    public PlayerStats(PlayerController playerController)
    {
        m_PlayerController = playerController;

        MaxHelth = playerController.GetMaxHelth();
        MaxStamina = playerController.GetMaxStamina();

        CurrentHelth = MaxHelth;
        CurrentStamina = MaxStamina;
    }

    public void ApplyDamage(float damage)
    {
        if (damage < 0) return; 

        if (CurrentHelth - damage < 0)
            CurrentHelth = 0;
        else
            CurrentHelth -= damage;

        if (CurrentHelth < 1)
            m_PlayerController.OnDeath?.Invoke();

        OnChangeHelth?.Invoke();
    }

    public void Treat(float helth)
    {
        if (helth < 0) return;

        if (CurrentHelth + helth > MaxHelth)
            CurrentHelth = MaxHelth;
        else
            CurrentHelth += helth;

        OnChangeHelth?.Invoke();
    }

    public void DecreaseStamina(float stamina)
    {
        if (stamina < 0) return;

        if (CurrentStamina - stamina < 0)
            CurrentStamina = 0;
        else
            CurrentStamina -= stamina;

        OnChangeStamina?.Invoke();
    }
    
    public void IncreaseStamina(float stamina)
    {
        if (stamina < 0) return;

        if (CurrentStamina + stamina > MaxStamina)
            CurrentStamina = MaxStamina;
        else
            CurrentStamina += stamina;

        OnChangeStamina?.Invoke();
    }
}
