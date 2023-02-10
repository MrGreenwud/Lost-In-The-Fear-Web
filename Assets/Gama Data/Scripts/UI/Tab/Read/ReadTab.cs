using TMPro;
using UnityEngine;

public class ReadTab : Tab
{
    [SerializeField] private TextMeshProUGUI m_TextMeshPro;

    private void Awake()
    {
        p_View = new ReadTabView(this);
    }

    public void Read(string Text)
    {
        m_TextMeshPro.text = Text;
    }
}
