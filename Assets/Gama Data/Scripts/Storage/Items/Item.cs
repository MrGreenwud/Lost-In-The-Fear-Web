using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item", fileName = "BaseItem", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField] private ItemType m_Type;

    [Space(10)]

    [SerializeField] private Sprite m_Icon;
    [SerializeField] private string m_ItemName;
    [SerializeField] private uint m_ID;
    [SerializeField] private GameObject m_Prefab;
    
    [Space(10)]

    [SerializeField] private bool m_IsReueble;
    [SerializeField] private bool m_IsStaceble;
    
    [Space(5)]

    [SerializeField] [Range(1, 100)] private int m_MaxStacSize = 1;

    public ItemType GetItemType() => m_Type;
    public Sprite GetIcon() => m_Icon;
    public string GetItemName() => m_ItemName;
    public uint GetID() => m_ID;
    public GameObject GetPrefab() => m_Prefab;

    public bool GetIsStacable() => m_IsStaceble;
    public bool GetIsReueble() => m_IsReueble;
    public int GetMaxStecSize() => m_MaxStacSize;

    public virtual void Use()
    {
        Debug.Log("Use: " + name);
    }
}

public enum ItemType
{
    Empty,
    Quest,
    Weapon,
    Сartridges,
    Protection,
    Medicine
}