using System;
using UnityEngine;

namespace BCTSTool.Localization
{
    [Serializable]
    public class LocalizationImage
    {
        [SerializeField] private uint m_ID;
        [SerializeField] private Sprite m_Sprite;

        public uint GetID() => m_ID;
        public Sprite GetSprite() => m_Sprite;
    }
}
