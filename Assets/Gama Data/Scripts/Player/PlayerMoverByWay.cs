using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class PlayerMoverByWay : MonoBehaviour
{
    [Inject] private readonly PlayerController m_PlayerController;

    [SerializeField] private float m_ScreenCloseTime = 3;
    [SerializeField] private Animator[] m_DarkScreen;
    private Transform m_Transform;

    private void Awake()
    {
        m_Transform = m_PlayerController.transform;
    }

    public void Teleport(Vector3 position, float endTime)
    {
        StartCoroutine(StopMove(position, endTime));

        m_PlayerController.Freez();

        for (int i = 0; i < m_DarkScreen.Length; i++)
            m_DarkScreen[i].SetBool("isOpen", false);
    }

    IEnumerator StopMove(Vector3 position, float time)
    {
        yield return new WaitForSeconds(m_ScreenCloseTime);

        m_Transform.position = position;

        yield return new WaitForSeconds(time);

        for (int i = 0; i < m_DarkScreen.Length; i++)
            m_DarkScreen[i].SetBool("isOpen", true);

        m_PlayerController.UnFreez();
    }
}
