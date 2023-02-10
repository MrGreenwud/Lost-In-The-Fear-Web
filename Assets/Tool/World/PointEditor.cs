using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BCTSTool.Editor;

#if UNITY_EDITOR

namespace BCTSTool.World
{
    [Serializable]
    public class Point
    {
        public Vector3 Position;
        public int LineIndex;

        public Point(Vector3 position, int lineIndex)
        {
            Position = position;
            LineIndex = lineIndex;
        }
    }

    [Serializable]
    public class Line
    {
        [SerializeField] private List<Point> m_Points;

        public Line()
        {
            m_Points = new List<Point>(0);
        }

        public int GetCount() => m_Points.Count;

        public Point[] GetPoints()
        {
            Point[] newPoints = new Point[m_Points.Count];

            for(int i = 0; i < m_Points.Count; i++)
                newPoints[i] = m_Points[i];

            return newPoints;
        }

        public void Add(Point newPoint) => m_Points.Add(newPoint);
        public void Insert(int index, Point newPoint) => m_Points.Insert(index, newPoint);
        public void ChangeValue(int index, Vector3 newPoition) => m_Points[index].Position = newPoition;

        public Point GetLastPoint() => m_Points[m_Points.Count - 1];
    }

    [ExecuteInEditMode]
    public class PointEditor : MonoBehaviour
    {
        public enum Mode
        {
            View,
            Edit,
        }

        public enum Tool
        {
            Null,
            Add,
            Divid,
            Move,
            Line_Selection
        }

        [SerializeField] private Mode m_Mode;
        [SerializeField] private Tool m_Tool;

        [Space(10)]

        [SerializeField] private bool m_IsSnap = true;

        [SerializeField] private LayerMask m_GroundLayerMask;
        [SerializeField] private List<Line> m_Lines;

        [SerializeField] private List<int> m_SelectionPoint;
        [SerializeField] private int m_ActiveLine;

        private Vector2 m_MouseOriginPositionInScreen;
        private Vector3 m_MouseOriginPositionInWorld;

        private Vector2 m_MousePosition;
        private Vector3 m_MousePositionInWorld;

        private List<Vector3> m_PointsOriginPosition = new List<Vector3>();
        private Camera m_Camera;

        public Action OnAddPoint;
        public Action OnMovePoint;

        public Line[] GetLines()
        {
            Line[] lines = new Line[m_Lines.Count];

            for(int i = 0; i < m_Lines.Count; i++)
                lines[i] = m_Lines[i];

            return lines;
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += SceneGUIUpdate;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= SceneGUIUpdate;
        }

        private void SceneGUIUpdate(SceneView sceneView)
        {
            if (Selection.activeGameObject != gameObject) return;
            
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            m_Camera = sceneView.camera;
            m_MousePosition = Event.current.mousePosition;

            if (m_Mode == Mode.Edit)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(m_MousePosition);
                bool isHit = Physics.Raycast(m_Camera.transform.position, ray.direction, out RaycastHit hit, 1000);
                m_MousePositionInWorld = hit.point;

                KeyPressedHandler.PressedKeyHandle(Event.current, KeyCode.LeftShift);
                KeyPressedHandler.PressedKeyHandle(Event.current, KeyCode.LeftControl);

                Move(Event.current, isHit, hit);

                SelectPoint(Event.current);
                SelectLine(Event.current);

                AddPoint(Event.current, isHit, hit);
                Divid(Event.current, isHit, hit);

                if (Event.current.isMouse && Event.current.button == 0 && Event.current.type == EventType.MouseDown)
                    m_MouseOriginPositionInWorld = hit.point;
            }

            SwichMode(Event.current);
            SwichTool(Event.current);
        }

        private void SwichMode(Event e)
        {
            if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.X)
            {
                m_SelectionPoint.Clear();

                if (m_Mode == Mode.View)
                    m_Mode = Mode.Edit;
                else if (m_Mode == Mode.Edit)
                    m_Mode = Mode.View;
            }
        }

        private void SwichTool(Event e)
        {
            if (m_Mode == Mode.View) return;

            if (e.isKey && e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Z)
                {
                    if (m_Tool == Tool.Add)
                        m_Tool = Tool.Null;
                    else
                        m_Tool = Tool.Add;
                }
                else if (e.keyCode == KeyCode.V)
                {
                    if (m_Tool == Tool.Divid)
                        m_Tool = Tool.Null;
                    else
                        m_Tool = Tool.Divid;
                }
                else if(e.keyCode == KeyCode.N)
                {
                    if (m_Tool == Tool.Line_Selection)
                        m_Tool = Tool.Null;
                    else
                        m_Tool = Tool.Line_Selection;
                }
            }
        }

        private void ChangeTool(Tool newTool)
        {
            if (newTool == Tool.Null)
                if (m_Tool == Tool.Add || m_Tool == Tool.Divid) return;

            m_Tool = newTool;
        }

        private void SelectPoint(Event e)
        {
            if (m_Mode == Mode.View) return;
            if (m_Tool == Tool.Line_Selection) return;
            if (m_Lines.Count == 0) return;

            if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown)
            {
                Point[] points = m_Lines[m_ActiveLine].GetPoints();

                for (int j = 0; j < points.Length; j++)
                {
                    Vector2 pointProjection = HandleUtility.WorldToGUIPoint(points[j].Position);

                    if (Vector2.Distance(pointProjection, m_MousePosition) > 8f) continue;

                    if (KeyPressedHandler.s_KeyPresseds.GetPressedState(KeyCode.LeftShift))
                    {
                        m_SelectionPoint.Add(j);
                    }
                    else
                    {
                        m_SelectionPoint.Clear();
                        m_SelectionPoint.Add(j);
                    }

                    return;
                }

                if (!KeyPressedHandler.s_KeyPresseds.GetPressedState(KeyCode.LeftShift))
                {
                    if (m_Tool == Tool.Null)
                        m_SelectionPoint.Clear();

                    ChangeTool(Tool.Null);
                }
            }
        }

        private void SelectLine(Event e)
        {
            if (m_Mode == Mode.View) return;
            if (m_Tool != Tool.Line_Selection) return;

            if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown)
            {
                for (int i = 0; i < m_Lines.Count; i++)
                {
                    Point[] points = m_Lines[i].GetPoints();

                    for (int j = 0; j < points.Length; j++)
                    {
                        Vector2 pointProjection = HandleUtility.WorldToGUIPoint(points[j].Position);

                        if (Vector2.Distance(pointProjection, m_MousePosition) > 8f) continue;

                        m_ActiveLine = points[j].LineIndex;
                    }
                }
            }
        }

        private void AddPoint(Event e, bool isHit, RaycastHit hit)
        {
            if (m_Mode == Mode.View) return;
            if (m_Tool != Tool.Add) return;

            if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown)
            {
                if (isHit)
                {
                    if (m_Lines.Count == 0)
                        m_Lines.Add(new Line());

                    if (KeyPressedHandler.s_KeyPresseds.GetPressedState(KeyCode.LeftControl))
                    {
                        m_Lines.Add(new Line());
                        m_ActiveLine = m_Lines.Count - 1;
                    }

                    m_Lines[m_ActiveLine].Add(new Point(hit.point, m_ActiveLine));
                    OnAddPoint?.Invoke();
                }
            }
        }

        private void Divid(Event e, bool isHit, RaycastHit hit)
        {
            if (m_Mode == Mode.View) return;
            if (m_Tool != Tool.Divid) return;

            if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown)
            {
                if (m_SelectionPoint.Count < 2)
                {
                    m_Tool = Tool.Null;
                    return;
                }

                if (isHit)
                {
                    int index = m_SelectionPoint[1];

                    if (m_SelectionPoint[1] < m_SelectionPoint[0])
                        index = m_SelectionPoint[0];

                    if (m_Tool == Tool.Divid)
                    {
                        m_Lines[m_ActiveLine].Insert(index, new Point(hit.point, m_ActiveLine));
                        OnAddPoint?.Invoke();
                    }
                }
            }
        }

        private void Move(Event e, bool isHit, RaycastHit hit)
        {
            if (m_SelectionPoint.Count == 0) return;

            if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.G)
            {
                ChangeTool(Tool.Move);

                m_MouseOriginPositionInScreen = m_MousePosition;
                m_MouseOriginPositionInWorld = hit.point;

                if(m_PointsOriginPosition != null)
                    m_PointsOriginPosition.Clear();

                Point[] points = m_Lines[m_ActiveLine].GetPoints();

                for (int i = 0; i < m_SelectionPoint.Count; i++)
                    m_PointsOriginPosition.Add(points[m_SelectionPoint[i]].Position);
            }

            if (m_Tool == Tool.Move)
            {
                if (isHit)
                {
                    if (e.isMouse && e.type == EventType.MouseMove)
                    {
                        Point[] points = m_Lines[m_ActiveLine].GetPoints();

                        for (int i = 0; i < m_SelectionPoint.Count; i++)
                        {
                            Vector3 point = points[m_SelectionPoint[i]].Position;

                            point.x = m_PointsOriginPosition[i].x - (m_MouseOriginPositionInWorld.x - hit.point.x);
                            point.y = m_PointsOriginPosition[i].y - (m_MouseOriginPositionInWorld.y - hit.point.y);
                            point.z = m_PointsOriginPosition[i].z - (m_MouseOriginPositionInWorld.z - hit.point.z);

                            if (m_IsSnap)
                            {
                                Vector2 pointProject = HandleUtility.WorldToGUIPoint(point);
                                Ray pointProjectRay = HandleUtility.GUIPointToWorldRay(pointProject);

                                if (Physics.Raycast(pointProjectRay.GetPoint(0), pointProjectRay.direction, out RaycastHit projectHit, 1000))
                                    point = projectHit.point;
                            }

                            m_Lines[m_ActiveLine].ChangeValue(m_SelectionPoint[i], point);
                        }
                    }
                }
            }

            if (e.isMouse && e.type == EventType.MouseDown && e.button == 0 && m_Tool == Tool.Move)
            {
                OnMovePoint?.Invoke();
                ChangeTool(Tool.Null);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Camera == null) return;

            const float inaccuracy = 0.2f;
            const float fenceLength = 3.1f;

            for (int i = 0; i < m_Lines.Count; i++)
            {
                Point[] points = m_Lines[i].GetPoints();

                for (int j = 0; j < points.Length; j++)
                {
                    if (m_Mode == Mode.Edit)
                    {
                        Gizmos.color = Color.gray;

                        if (m_Tool == Tool.Line_Selection)
                        {
                            if (i == m_ActiveLine)
                                Gizmos.color = Color.green;
                        }
                        else
                        {
                            if (i == m_ActiveLine)
                                Gizmos.color = Color.blue;
                        }
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }

                    Gizmos.DrawSphere(points[j].Position, 0.2f);

                    if (j < 1) continue;

                    CustemGizmos.DrowLineByMultipleNumber(points[j - 1].Position, points[j].Position,
                                    Color.green, Color.red, fenceLength, inaccuracy);
                }
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(m_MousePosition);

            if (Physics.Raycast(m_Camera.transform.position, ray.direction, out RaycastHit hit, 1000))
            {
                if (m_Mode == Mode.Edit)
                {
                    if (m_Tool == Tool.Add)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(hit.point, 0.2f);
                    }
                    else if (m_Tool == Tool.Divid)
                    {
                        if (m_SelectionPoint.Count != 2) return;

                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(hit.point, 0.2f);

                        Point[] points = m_Lines[m_ActiveLine].GetPoints();

                        for (int i = 0; i < m_SelectionPoint.Count; i++)
                        {
                            CustemGizmos.DrowLineByMultipleNumber(points[m_SelectionPoint[i]].Position, hit.point,
                                    Color.green, Color.red, fenceLength, inaccuracy);
                        }
                    }
                }
            }

            if (m_Mode == Mode.Edit)
            {
                if (m_Tool == Tool.Add)
                {
                    if (!KeyPressedHandler.s_KeyPresseds.GetPressedState(KeyCode.LeftControl))
                    {
                        if (m_Lines.Count > 0)
                        {
                            if (m_Lines[m_ActiveLine].GetCount() > 0)
                            {
                                CustemGizmos.DrowLineByMultipleNumber(m_Lines[m_ActiveLine].GetLastPoint().Position, hit.point, 
                                    Color.green, Color.red, 3.1f, inaccuracy);

                                Gizmos.DrawLine(m_Lines[m_ActiveLine].GetLastPoint().Position, hit.point);
                            }
                        }
                    }
                }
                else
                {
                    if (m_Lines.Count > 0)
                    {
                        Point[] points = m_Lines[m_ActiveLine].GetPoints();

                        for (int i = 0; i < m_SelectionPoint.Count; i++)
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawSphere(points[m_SelectionPoint[i]].Position, 0.2f);
                        }
                    }
                }


                if (m_Tool == Tool.Move)
                {
                    Color debug = Color.white;
                    debug.a = 0.2f;

                    Gizmos.color = debug;
                    Gizmos.DrawSphere(hit.point, 0.2f);
                    Gizmos.DrawSphere(m_MouseOriginPositionInWorld, 0.2f);

                    Gizmos.DrawLine(hit.point, m_MouseOriginPositionInWorld);
                }
            }
        }
    }
}

namespace BCTSTool.Editor
{
    public struct KeyPreesed
    {
        public KeyCode KeyCode { get; }
        public bool PressedState { get; set; }

        public KeyPreesed(KeyCode keyCode, bool pressedState = false)
        {
            KeyCode = keyCode;
            PressedState = pressedState;
        }
    }

    public class KeyPresseds
    {
        private List<KeyPreesed> m_KeyPreeseds = new List<KeyPreesed>();

        public void Add(KeyCode keyCode, bool pressedState = false)
        {
            m_KeyPreeseds.Add(new KeyPreesed(keyCode, pressedState));
        }

        public void Add(KeyPreesed keyPreesed)
        {
            m_KeyPreeseds.Add(keyPreesed);
        }

        public int GetCount()
        {
            return m_KeyPreeseds.Count;
        }

        public bool ContainsKey(KeyCode keyCode)
        {
            for(int i = 0; i < m_KeyPreeseds.Count; i++)
            {
                if (m_KeyPreeseds[i].KeyCode == keyCode)
                    return true;
            }

            return false;
        }

        public void ChangePressed(KeyCode keyCode, bool newPressed)
        {
            for (int i = 0; i < m_KeyPreeseds.Count; i++)
            {
                if (m_KeyPreeseds[i].KeyCode == keyCode)
                {
                    KeyPreesed tempKeyPressed = new KeyPreesed(m_KeyPreeseds[i].KeyCode, m_KeyPreeseds[i].PressedState);
                    tempKeyPressed.PressedState = newPressed;
                    
                    m_KeyPreeseds[i] = tempKeyPressed;
                    return;
                }
            }
        }

        public bool GetPressedState(KeyCode keyCode)
        {
            for (int i = 0; i < m_KeyPreeseds.Count; i++)
            {
                if (m_KeyPreeseds[i].KeyCode == keyCode)
                    return m_KeyPreeseds[i].PressedState;
            }

            return false;
        }
    }

    [ExecuteInEditMode]
    public static class KeyPressedHandler
    {
        public static KeyPresseds s_KeyPresseds = new KeyPresseds();

        public static void PressedKeyHandle(Event e, KeyCode keyCode)
        {
            if (e.isKey && e.keyCode == keyCode)
            {
                if(s_KeyPresseds.ContainsKey(keyCode) == false)
                    s_KeyPresseds.Add(keyCode);

                if (e.type == EventType.KeyDown)
                    s_KeyPresseds.ChangePressed(keyCode, true);
                else if (e.type == EventType.KeyUp)
                    s_KeyPresseds.ChangePressed(keyCode, false);
            }
        }
    }
}

#endif