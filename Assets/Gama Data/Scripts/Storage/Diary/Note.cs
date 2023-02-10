using UnityEngine;

[CreateAssetMenu(menuName = "Item/Note", fileName = "BaseNote", order = 1)]
public class Note : Item
{
    [SerializeField] private string m_Text;

    public string GetText() => m_Text;
}
