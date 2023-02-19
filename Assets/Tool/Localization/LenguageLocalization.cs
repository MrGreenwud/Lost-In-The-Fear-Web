using UnityEngine;

namespace BCTSTool.Localization
{
    [CreateAssetMenu(fileName = "leng", menuName = "Localizatin")]
    public class LenguageLocalization : ScriptableObject
    {
        [SerializeField] private LocalizationText[] m_Text;
        [SerializeField] private LocalizationImage[] m_Image;

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

        public Sprite GetSpriteByID(uint id)
        {
            for (int i = 0; i < m_Image.Length; i++)
            {
                if (m_Image[i].GetID() == id)
                    return m_Image[i].GetSprite();
            }

            Debug.LogError($"This Lenguage file dosnt have sprite with index: {id}!");
            return null;
        }
    }
}
