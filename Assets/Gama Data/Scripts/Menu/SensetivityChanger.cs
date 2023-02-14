using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SensetivityChanger : MonoBehaviour
{
    private Slider m_Slider;

    public void Awake()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.value = Settings.s_Sensetivity / Settings.s_MaxSensetivity;
    }

    public void Change()
    {
        Settings.SetSensetivity(m_Slider.value);
        Debug.Log(Settings.s_Sensetivity);
    }
}
