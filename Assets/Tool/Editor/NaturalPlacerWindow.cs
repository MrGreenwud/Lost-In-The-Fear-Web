using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BCTSTool.DubugGizmos;

namespace BCTSTool.World
{
    [ExecuteInEditMode]
    public class NaturalPlacerWindow : EditorWindow
    {
        public enum Mode { Count, Density }
        public enum GroundLayers
        { 
            Layer0,
            Layer1,
            Layer2,
            Layer3,
            Layer4,
            Layer5,
            Layer6,
            Layer7,
            Layer8,
            Layer9,
            Layer10,
            Layer11,
            Layer12,
            Layer13,
            Layer14,
            Layer15,
            Layer16,
            Layer17,
            Layer18,
            Layer20,
            Layer21,
            Layer22,
            Layer23,
            Layer24,
            Layer25,
            Layer26,
            Layer27,
            Layer28,
            Layer29,
            Layer30,
            Layer31,
            Layer32,
        };

        private NaturalPlacer m_TreePlacer;
        private GizmonsRenderer m_GizmonsRenderer;
        private GameObject m_Temp;

        private Vector2 m_MousePoition;
        private Camera m_Camera;
        private Vector3 m_Position;

        private bool m_IsRemove = false;
        private bool m_IsPaint = false;

        private static GroundLayers s_GroundLayers;
        private static float s_Radius = 5;
        private static Mode s_Mode;
        private static float s_Density = 1;
        private static Vector2Int s_Count = new Vector2Int(5, 10);
        private static Naturals s_Trees;

        private static EditorWindow window;

        [MenuItem("Window/World Editor/Natural Placer _b")]
        public static void ShowWindow()
        {
            window = GetWindow<NaturalPlacerWindow>();
            window.titleContent = new GUIContent("Natural Placer");

            Vector2 windowSize = new Vector2(300, 210);

            window.minSize = windowSize;
            window.maxSize = windowSize;
        }

        private void OnEnable()
        {
            GameObject temp = new GameObject("TEMP");
            m_Temp = temp;

            GameObject treePlacer = new GameObject("Tree Placer");
            treePlacer.AddComponent<NaturalPlacer>();
            treePlacer.transform.parent = temp.transform;
            m_TreePlacer = treePlacer.GetComponent<NaturalPlacer>();

            GameObject gizmonsRenderer = new GameObject("Gizmons Renderer");
            gizmonsRenderer.AddComponent<GizmonsRenderer>();
            gizmonsRenderer.transform.parent = temp.transform;
            m_GizmonsRenderer = gizmonsRenderer.GetComponent<GizmonsRenderer>();

            m_GizmonsRenderer.OnDrowGizmos += OnDrowGizmos;

            SceneView.duringSceneGui += OnUpdate;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.ObjectField(" Tree Generator ", m_TreePlacer, typeof(GameObject), false);

            EditorGUILayout.Space();

            s_Radius = EditorGUILayout.FloatField(" Brush Radius ", s_Radius);

            s_GroundLayers = (GroundLayers)EditorGUILayout.EnumPopup(" Ground Layers: ", s_GroundLayers);

            EditorGUILayout.Space();

            s_Mode = (Mode)EditorGUILayout.EnumPopup("Mode: ", s_Mode);

            if (s_Mode == Mode.Count)
                s_Count = EditorGUILayout.Vector2IntField("Count : min : max", s_Count, GUILayout.Width(200));
            else
                s_Density = EditorGUILayout.FloatField(" Demnsity: ", s_Density);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField($"Trees count: {m_TreePlacer.NaturalsCount}");

            EditorGUILayout.Space();

            s_Trees = EditorGUILayout.ObjectField(s_Trees, typeof(Naturals), false) as Naturals;

            EditorGUILayout.Space();

            if (s_Mode == Mode.Density)
            {
                if (GUILayout.Button("Generate"))
                    Generat();
            }
        }

        private void OnUpdate(SceneView sceneView)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            m_Camera = SceneView.GetAllSceneCameras()[0];
            m_MousePoition = Event.current.mousePosition;

            var input = Event.current;

            if(input.isKey && input.keyCode == KeyCode.LeftControl)
            {
                if (input.type == EventType.KeyDown)
                    m_IsRemove = true;
                else if (input.type == EventType.KeyUp)
                    m_IsRemove = false;
            }

            if (input.isMouse && input.button == 0)
                m_IsPaint = input.type == EventType.MouseDrag;

            if (m_IsPaint)
            {
                Tools.current = Tool.View;
                Event.current.Use();

                if (m_IsRemove)
                    Remove();
                else
                    Generat();
            }
            else
            {
                Tools.current = Tool.None;
            }
        }

        private void OnDisable()
        {
            if (m_TreePlacer != null)
                DestroyImmediate(m_TreePlacer.gameObject);

            if (m_GizmonsRenderer != null)
            {
                m_GizmonsRenderer.OnDrowGizmos -= OnDrowGizmos;
                DestroyImmediate(m_GizmonsRenderer.gameObject);
            }

            DestroyImmediate(m_Temp);

            SceneView.duringSceneGui -= OnUpdate;
        }

        private void OnDrowGizmos()
        {
            if (!Application.IsPlaying(m_GizmonsRenderer.gameObject))
            {
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(m_MousePoition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(hit.point, hit.normal, 0.5f);
                Handles.DrawWireDisc(hit.point, hit.normal, 1f);
                Handles.DrawWireDisc(hit.point, hit.normal, s_Radius);

                m_Position = hit.point;
            }
        }

        private void Generat()
        {
            if (s_Mode == Mode.Count)
                m_TreePlacer.GenerateByCount(s_Radius, m_Position, s_Trees, s_Count);
            else
                m_TreePlacer.GenerateByDensity(s_Radius, m_Position, s_Trees, s_Density);
        }

        private void Remove()
        {
            m_TreePlacer.Remove(s_Radius, m_Position, s_Count);
        }

        /*
        private void OnDrawGizmos()
        {
            //if (!Application.IsPlaying(gameObject))
            //{
            //    EditorApplication.QueuePlayerLoopUpdate();
            //    SceneView.RepaintAll();
            //}

            /*
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out RaycastHit hit2, 200, m_GroundLayer))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(hit2.point, 0.5f);

                Handles.color = Color.blue;
                Handles.DrawWireDisc(hit2.point, hit2.normal, 1);
                Handles.DrawWireDisc(hit2.point, hit2.normal, 5);
            }
        }
        */
    }
}
