using UnityEditor;
using UnityEngine;

public class TabView
{
    protected Tab p_Tab;

    public TabView(Tab tab)
    {
        p_Tab = tab;
    }

    public virtual void Show() 
    {
        p_Tab.GetTab().SetActive(true);
    }
    
    public virtual void Hide() 
    {
        p_Tab.GetTab().SetActive(false);
    }
}
