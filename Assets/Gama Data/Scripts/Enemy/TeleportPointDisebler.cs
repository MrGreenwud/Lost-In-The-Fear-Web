using System.Collections.Generic;
using UnityEngine;

public class TeleportPointDisebler : MonoBehaviour
{
    [SerializeField] private LayerMask m_TeleportPointsLayer;
    [SerializeField] private List<TeleportPoint> m_TeleportPoints;

    private void OnEnable()
    {
        RaycastHit[] teleportPoints = Physics.BoxCastAll(transform.position, transform.lossyScale * 0.5f,
            Vector3.up * 0.1f, transform.rotation, m_TeleportPointsLayer);

        for(int i = 0; i < teleportPoints.Length; i++)
        {
            if (teleportPoints[i].collider.TryGetComponent<TeleportPoint>(out TeleportPoint teleportPoint))
            {
                if (teleportPoint.gameObject.activeSelf == true)
                {
                    m_TeleportPoints.Add(teleportPoint);
                    teleportPoint.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnDisable()
    {
        if(m_TeleportPoints.Count > 0)
        {
            int count = m_TeleportPoints.Count;

            while (count > 0)

            for (int i = 0; i < m_TeleportPoints.Count; i++, count--)
            {
                m_TeleportPoints[i].gameObject.SetActive(true);
                m_TeleportPoints.Remove(m_TeleportPoints[i]);
            }
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
