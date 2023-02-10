using UnityEngine;

[CreateAssetMenu(menuName = "Item/Protection", fileName = "ProtectionBase", order = 1)]
public class ProtectionItem : Item
{
    [Space(10)]
    [Header("Protaction Settings")]

    [SerializeField] private Protation m_ProtationType;

    [Space(5)]

    [SerializeField] private float m_StanTime = 30f;
    [SerializeField] private float m_StanDistence = 2f;

    public override void Use()
    {
        ItemUser.Instance.Protactor.SetPotactionType(m_ProtationType);
        ItemUser.Instance.Protactor.SetStanTime(m_StanTime);
        ItemUser.Instance.Protactor.SetStanDistance(m_StanDistence);

        base.Use();
    }
}

public enum Protation
{
    Null,
    Salt,
    Holly_Water
}