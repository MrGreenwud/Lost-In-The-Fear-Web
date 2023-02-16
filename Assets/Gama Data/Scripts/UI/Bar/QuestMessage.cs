using UnityEngine;
using TMPro;
using BCTSTool.Localization;

[RequireComponent(typeof(TextMeshProUGUI))]
public class QuestMessage : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField] private PlayerInteractor m_PlayerInteractor;
    [SerializeField] private LenguageLocalization m_Localization;

    private float m_AlphaText = 1f;

    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        m_PlayerInteractor.OnIteract += Show;
    }

    private void Update()
    {
        if (m_TextMeshProUGUI.color.a > 0)
            ChengeAlpha(m_AlphaText -= Time.deltaTime * 0.5f);
    }

    private void Show(Interacteble interacteble)
    {
        if (interacteble is QuestCompliter questCompliter)
            Show(questCompliter.GetMessageID());
    }

    public void Show(uint id)
    {
        if (id == 0)
            return;

        m_AlphaText = 1;
        ChengeAlpha(1);

        m_TextMeshProUGUI.text = m_Localization.GetTextByID(id);
    }

    private void ChengeAlpha(float alpha)
    {
        Color newColor = m_TextMeshProUGUI.color;

        newColor.a = alpha;
        m_TextMeshProUGUI.color = newColor;
    }
}
