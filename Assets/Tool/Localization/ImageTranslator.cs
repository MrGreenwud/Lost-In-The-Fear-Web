using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BCTSTool.Localization
{
    [RequireComponent(typeof(Image))]
    public class ImageTranslator : MonoBehaviour
    {
        [SerializeField] private SettingsChanger m_SettingsChanger;
        [SerializeField] private LenguageLocalization m_Lenguage;

        private Image m_Image;
        [SerializeField] private uint m_ID;

        private void Awake()
        {
            m_Image = GetComponent<Image>();

            if (m_SettingsChanger != null)
                m_SettingsChanger.OnChangeLenguage.AddListener(Translat);

            Translat();
        }

        public void Translat()
        {
            Debug.Log("Translat " + gameObject.name);

            if(Settings.s_Lenguage != null)
                m_Image.sprite = Settings.s_Lenguage.GetSpriteByID(m_ID);
            else
                m_Image.sprite = m_Lenguage.GetSpriteByID(m_ID);

            Debug.Log(Settings.s_Lenguage);
        }
    }
}
