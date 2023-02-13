using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    #region Models

    public CharacterController CharacterController { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public PlayerMover PlayerMover { get; private set; }
    public PlayerCroucher PlayerCroucher { get; private set; }
    public PlayerJamper PlayerJamper { get; private set; }

    public PlayerGravity PlayerGravity { get; private set; }

    public PlayerStateController PlayerStateController { get; private set; }
    public PlayerStats PlayerStats { get; private set; }

    #endregion

    #region Viwe

    public PlayerStatsView PlayerStatsView { get; private set; }

    [Header("Stats Settings View")]

    [Space(2)]

    [Header("Helth")]

    [SerializeField] private PostProcessVolume m_PostProcessVolume;
    [SerializeField] private float[] m_HelthVignetteIntensitys;

    [Space(5)]
    [Header("Stamina")]

    [SerializeField] private Image m_StaminaBar;
    [SerializeField] private Image m_StaminaBackGround;

    [SerializeField] private FoodSteps m_FoodSteps;

    public PostProcessVolume GetPostProcessVolume() => m_PostProcessVolume;
    public float[] GetHelthVignetteIntensitys() => m_HelthVignetteIntensitys;

    public Image GetStaminaBar() => m_StaminaBar;
    public Image GetStaminaBackGround() => m_StaminaBackGround;

    #endregion

    public bool IsMove { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsCrouch { get; private set; }
    public bool IsJump { get; private set; }

    [Header("Stats Settings")]

    [SerializeField] private float m_MaxHelth;
    [SerializeField] private float m_MaxStamina;

    [Space(5)]

    [SerializeField] private float m_StaminaDecreaseValue = 2;
    [SerializeField] private float m_StaminaIncreaseValue = 2;

    [Space(10)]
    [Header("Move Settings")]

    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] private float m_CrowlSpeed;

    [Space(5)]

    [SerializeField] private float m_CrouchHeight = 1;

    [Space(10)]
    [Header("Jamp Over Settings")]

    [SerializeField] private float m_JumpOverSpeed = 5;

    [SerializeField] private float m_MaxObstacleHight = 1;
    [SerializeField] private float m_MaxObstacleLong = 2;

    [SerializeField] private LayerMask m_ObstacleLayer;


    [Space(10)]
    [Header("Gravity")]

    [SerializeField] private float m_Gravity;
    [SerializeField] private LayerMask m_GroundLayer;

    private bool m_IsFreez;

    public Action OnDeath;

    public float GetMaxHelth() => m_MaxHelth;
    public float GetMaxStamina() => m_MaxStamina;

    public float GetWalkSpeed() => m_WalkSpeed;
    public float GetRunSpeed() => m_RunSpeed;
    public float GetCrowlSpeed() => m_CrowlSpeed;
    public float GetCrouchHeight() => m_CrouchHeight;

    public float GetJumpOverSpeed() => m_JumpOverSpeed;
    public float GetMaxObstacleHight() => m_MaxObstacleHight;
    public float GetMaxObstacleLong() => m_MaxObstacleLong;
    public LayerMask GetObstacleLayer() => m_ObstacleLayer;

    public float GetGravity() => m_Gravity;
    public LayerMask GetGroundLayer() => m_GroundLayer;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        InputHandler = GetComponent<InputHandler>();

        PlayerStats = new PlayerStats(this);

        PlayerMover = new PlayerMover(this);
        PlayerCroucher = new PlayerCroucher(this);
        PlayerJamper = new PlayerJamper(this);
        PlayerGravity = new PlayerGravity(this);

        PlayerStatsView = new PlayerStatsView(this);

        PlayerStateController = new PlayerStateController(this);    }

    private void OnEnable()
    {
        PlayerStats.OnChangeHelth += PlayerStatsView.ChangeHelth;
        PlayerStats.OnChangeStamina += PlayerStatsView.ChangeStamina;
    }

    private void OnDisable()
    {
        PlayerStats.OnChangeHelth -= PlayerStatsView.ChangeHelth;
        PlayerStats.OnChangeStamina -= PlayerStatsView.ChangeStamina;
    }

    private void Update()
    {
        UpdateLogic();
        UpdateViwe();
    }

    private void UpdateLogic()
    {
        if (m_IsFreez == false)
        {
            PlayerStateController.Update();

            if (PlayerGravity.ChackGround() == false)
                PlayerGravity.Fall();

            OnUpdateColition();
            ChangeBooleanStates();

            if (IsRun == true)
                PlayerStats.DecreaseStamina(m_StaminaDecreaseValue * Time.deltaTime);
            else if (InputHandler.GetRun() == false)
                PlayerStats.IncreaseStamina(m_StaminaIncreaseValue * Time.deltaTime);

            if (IsMove == true)
                m_FoodSteps.Step(IsRun);
        }
    }

    private void UpdateViwe()
    {
        PlayerStatsView.Update();
    }

    private void ChangeBooleanStates()
    {
        if (Mathf.Abs(InputHandler.GetMoveAxis().x) + Mathf.Abs(InputHandler.GetMoveAxis().y) != 0)
            IsMove = true;
        else
            IsMove = false;

        if (PlayerStats.CurrentStamina > 0 && PlayerStats.CurrentHelth > 1 && InputHandler.GetMoveAxis().y > 0)
            IsRun = InputHandler.GetRun();
        else
            IsRun = false;

        IsCrouch = InputHandler.GetCrouch();

        if (IsJump == false)
        {
            if (InputHandler.GetJump() == true)
                IsJump = PlayerJamper.CheckJumpOver();
        }
        else
        {
            if (PlayerJamper.JumpTCofisent > 0.9f)
                IsJump = false;
        }
    }

    private void OnUpdateColition()
    {
        //For colculate colition on stay
        if (IsMove == false && IsJump == false)
            CharacterController.Move(transform.forward * 0.001f * Time.deltaTime);

    }

    public void Freez() => m_IsFreez = true;

    public void UnFreez()
    {
        m_IsFreez = false;
        OnUpdateColition();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;

        PlayerGravity.OnDrawGizmos();
        PlayerJamper.OnDrawGizmos();
    }

#endif

    private void OnApplicationQuit()
    {
        PlayerStatsView.ResetView();
    }
}