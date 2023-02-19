using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using BCTSTool.Localization;

public class SettingsChanger : MonoBehaviour
{
    [SerializeField] private Slider m_SensetivitySlider;

    [Space(10)]

    [SerializeField] private LenguageLocalization m_Ru;
    [SerializeField] private LenguageLocalization m_Eu;

    public UnityEvent OnChangeLenguage;

    public void Awake()
    {
        m_SensetivitySlider.value = Settings.s_Sensetivity / Settings.s_MaxSensetivity;

        if(Settings.s_Lenguage == null)
            Settings.SwichLenguage(m_Ru);

        OnChangeLenguage?.Invoke();
    }

    public void ChangeSensetivity()
    {
        Settings.SetSensetivity(m_SensetivitySlider.value);
    }

    public void SwichLenguage(LenguageLocalization lenguage)
    {
        Settings.SwichLenguage(lenguage);
        OnChangeLenguage?.Invoke();
        Debug.Log(lenguage.name);
    }
}
