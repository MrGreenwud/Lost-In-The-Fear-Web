using UnityEngine;

public class RandomCameraPositionSetter : MonoBehaviour
{
    [SerializeField] private Transform[] m_Positions;
    [SerializeField] private Transform m_Camera;

    private void Awake()
    {
        int index = Random.Range(0, m_Positions.Length);

        m_Camera.position = m_Positions[index].position;
        m_Camera.rotation = m_Positions[index].rotation;

        Debug.Log(index);
    }
}
