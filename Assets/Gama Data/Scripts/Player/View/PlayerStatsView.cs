using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerStatsView
{
    private readonly PlayerController m_PlayerController;
    private readonly PlayerStats m_PlayerStats;

    private readonly float[] m_HelthVignetteIntensitys;
    private readonly Vignette m_Vignette;

    private readonly Image m_StaminaBar;
    private readonly Image m_StaminaBackGround;

    private float m_IntensityValue;
    private float m_IntensityValueView;

    private Color m_ColorAlphaTransperent;

    public PlayerStatsView(PlayerController playerController)
    {
        m_PlayerController = playerController;
        m_PlayerStats = playerController.PlayerStats;
        m_HelthVignetteIntensitys = playerController.GetHelthVignetteIntensitys();
        m_Vignette = playerController.GetPostProcessVolume().sharedProfile.GetSetting<Vignette>();

        m_StaminaBar = playerController.GetStaminaBar();
        m_StaminaBackGround = playerController.GetStaminaBackGround();

        m_ColorAlphaTransperent = m_StaminaBar.color;

        ChangeHelth();
    }

    public void ChangeHelth()
    {
        m_IntensityValue = m_HelthVignetteIntensitys[(int)(m_PlayerStats.MaxHelth - m_PlayerStats.CurrentHelth)];
    }

    public void ChangeStamina()
    {
        m_StaminaBar.fillAmount = m_PlayerStats.CurrentStamina / m_PlayerStats.MaxStamina;
    }

    public void Update()
    {
        m_IntensityValueView = Mathf.Lerp(m_IntensityValueView, m_IntensityValue, 10 * Time.deltaTime);
        
        float step = m_IntensityValueView - 0.15f;
        m_Vignette.intensity.value = Mathf.PingPong(Time.time * 0.1f, m_IntensityValueView - step) + step;

        if(m_PlayerController.IsRun == false && m_PlayerStats.CurrentStamina == m_PlayerStats.MaxStamina)
            m_ColorAlphaTransperent.a = Mathf.Lerp(m_ColorAlphaTransperent.a, 0.3f, 4 * Time.deltaTime);
        else
            m_ColorAlphaTransperent.a = Mathf.Lerp(m_ColorAlphaTransperent.a, 1, 10 * Time.deltaTime);

        m_StaminaBar.color = m_ColorAlphaTransperent;
        m_StaminaBackGround.color = m_ColorAlphaTransperent;
    }

    public void ResetView()
    {
        m_Vignette.intensity.value = 0;
    }
}
