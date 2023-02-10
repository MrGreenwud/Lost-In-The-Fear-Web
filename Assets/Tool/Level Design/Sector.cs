using UnityEngine;

public class Sector : MonoBehaviour
{
    private enum SectorTypes
    {
        Open,
        Close
    }

    [SerializeField] private string m_SectorName;
    [SerializeField] private SectorTypes SectorType;

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (SectorType == SectorTypes.Open)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, transform.lossyScale);

        Vector3 lablePosition = transform.position + new Vector3(0, transform.lossyScale.y, 0);
        UnityEditor.Handles.Label(lablePosition, m_SectorName);
    }

#endif

}
