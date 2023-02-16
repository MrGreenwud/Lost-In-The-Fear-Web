using System;
using UnityEngine;

namespace BCTSTool.Localization
{
    [Serializable]
    public class LocalizationText
    {
        [SerializeField] private uint m_ID;
        [SerializeField] [TextArea] private string m_Text;

        public uint GetID() => m_ID;
        public string GetText() => m_Text;
    }
}
