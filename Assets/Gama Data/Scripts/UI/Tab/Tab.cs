using UnityEngine;

public class Tab : MonoBehaviour
{
    protected TabView p_View;

    [SerializeField] private GameObject m_Tab;

    public GameObject GetTab() => m_Tab;

    public virtual void Open() 
    {
        p_View.Show();
    }

    public virtual void Close() 
    {
        p_View.Hide();
    }
}
