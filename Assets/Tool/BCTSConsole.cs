using UnityEngine;
using static BCTSTool.BCTSDebug;
using static BCTSTool.Math.RectMath;

namespace BCTSTool
{
    [ExecuteAlways]
    public class BCTSConsole : MonoBehaviour
    {
        [SerializeField] private GUIStyle m_Style;

        [Space(10)]
        [SerializeField] private int m_FontSize = 30;

        [Space(10)]
        [SerializeField] private Rect m_LabelRect = new Rect(600, 800, 0, 0);

        [Space(10)]
        [SerializeField] private Rect m_BoxRect = new Rect(600, 750, 600, 300);

        private Vector2 m_ScrollPosition;

        private Vector2 m_NativeSize = new Vector2(1920, 1080);

        [Space(10)]
        [SerializeField] private bool m_IsEnabled;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
                m_IsEnabled = !m_IsEnabled;
        }
        
        private void OnGUI()
        {
            if (m_IsEnabled == true)
            {
                float screenAsspect = (float)(Screen.width) / (float)(m_NativeSize.x);

                m_Style.fontSize = (int)(m_FontSize * screenAsspect);

                Rect newLableRect = Multiplay(m_LabelRect, screenAsspect);
                Rect newBoxRect = Multiplay(m_BoxRect, screenAsspect);

                GUI.Box(newLableRect, " ");
                
                GUI.Label(Add(newLableRect, new Rect(40, 5, 0, 0)), "BCTSConsole:", m_Style);
                GUI.Label(Add(newLableRect, new Rect(300, 5, 0, 0)), "Massege Count: " + s_MassegesCount, m_Style);
                GUI.Box(newBoxRect, " ");

                if (s_Masseges == null) return;

                m_ScrollPosition = GUI.BeginScrollView(newBoxRect, m_ScrollPosition, new Rect(0, 0, newBoxRect.width, 25 * s_MassegesCount));

                GUI.Label(new Rect(0, -25, 0, 0), s_Masseges, m_Style);

                GUI.EndScrollView();
            }
        }

        private void OnApplicationQuit()
        {
            ClearMessege();
        }
    }
}