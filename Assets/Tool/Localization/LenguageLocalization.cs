using UnityEngine;

namespace BCTSTool.Localization
{
    [CreateAssetMenu(fileName = "leng", menuName = "Localizatin")]
    public class LenguageLocalization : ScriptableObject
    {
        [SerializeField] private LocalizationText[] m_Text;

        public string GetTextByID(uint id)
        {
            for (int i = 0; i < m_Text.Length; i++)
            {
                if (m_Text[i].GetID() == id)
                    return m_Text[i].GetText();
            }

            Debug.LogError($"This Lenguage file dosnt have text with index: {id}!");
            return "Error!";
        }
    }
}
