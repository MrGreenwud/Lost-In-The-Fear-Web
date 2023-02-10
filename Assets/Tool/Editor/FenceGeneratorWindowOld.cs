using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using BCTSTool.UIElements;

namespace BCTSTool.UIElements
{
    public static class CustomeUIElements
    {
        public static Button CreatButton(string text, VisualElement visualElement)
        {
            Button newButton = new Button();
            newButton.text = text;
            visualElement.Add(newButton);

            return newButton;
        }
    }
}

namespace BCTSTool.World
{
    public class FenceGeneratorWindowOld : EditorWindow
    {
        private static List<string> s_ChosesGenerateMode = new List<string>() { "Points", "Line" };

        private static FenceRepitor s_FenceRepitor;

        private static Toggle s_GenerateInRealTime;
        private static DropdownField s_GeneratMode;

        private static Label s_FenceRepitorName;

        [MenuItem("Window/World Editor/Fence Generator")]
        public static void ShowWindow()
        {
            s_GenerateInRealTime = new Toggle();
            s_GeneratMode = new DropdownField(s_ChosesGenerateMode, 0);
            s_FenceRepitorName = new Label();

            EditorWindow window = GetWindow<FenceGeneratorWindowOld>();
            window.titleContent = new GUIContent("Fence Generator");

            Vector2 windowSize = new Vector2(230, 450);

            window.minSize = windowSize;
            window.maxSize = windowSize;

            SelectFenceRepitor();
            Selection.selectionChanged += SelectFenceRepitor;
        }

        private void CreateGUI()
        {
            rootVisualElement.Add(new Label(" "));

            if (s_FenceRepitor != null)
            {
                rootVisualElement.Add(s_FenceRepitorName);
                rootVisualElement.Add(new Label(" "));
            }

            CustomeUIElements.CreatButton("Select Fence Repitor", rootVisualElement).clicked += Select;

            rootVisualElement.Add(new Label(" "));

            s_GenerateInRealTime.label = "Generat In Real Time";
            rootVisualElement.Add(s_GenerateInRealTime);

            s_GeneratMode.label = "Generate Mode";
            rootVisualElement.Add(s_GeneratMode);

            rootVisualElement.Add(new Label(" "));

            VisualElement Buttons = new VisualElement();

            CustomeUIElements.CreatButton("Creat Point", Buttons).clicked += CreatePoint;
            CustomeUIElements.CreatButton("Divide", Buttons).clicked += Divide;

            Buttons.Add(new Label(" "));

            CustomeUIElements.CreatButton("Covert Point To Line", Buttons).clicked += CovertPointToLine;
            CustomeUIElements.CreatButton("Covert Line To Point", Buttons).clicked += CovertLineToPoint;
            CustomeUIElements.CreatButton("Covert Old Points To New", Buttons).clicked += CovertOldPointsToNew;

            Buttons.Add(new Label(" "));

            CustomeUIElements.CreatButton("Remove Void Points", Buttons).clicked += RemoveVoidPoints;
            CustomeUIElements.CreatButton("Remove No Valit Points In Line", Buttons).clicked += RemoveNoValitPointsInLine;

            Buttons.Add(new Label(" "));

            CustomeUIElements.CreatButton("Remove All Points", Buttons).clicked += RemoveAllPoints;
            CustomeUIElements.CreatButton("Remove All Lines", Buttons).clicked += RemoveAllLines;

            Buttons.Add(new Label(" "));

            CustomeUIElements.CreatButton("Generate", Buttons).clicked += Generate;
            CustomeUIElements.CreatButton("Generate All Selected", Buttons).clicked += GenerateAllSelected;
            CustomeUIElements.CreatButton("Destroy All Fences", Buttons).clicked += DestroyAllFences;

            rootVisualElement.Add(Buttons);
        }

        private static void SetValue()
        {
            if (s_FenceRepitor == null)
            {
                s_FenceRepitorName.visible = false;
                return;
            }

            s_FenceRepitorName.text = s_FenceRepitor.gameObject.name;
            s_FenceRepitorName.visible = true;

            s_GenerateInRealTime.value = s_FenceRepitor.m_GenerateInRealTime;

            if (s_FenceRepitor.m_LineMode.ToString() == s_ChosesGenerateMode[0])
                s_GeneratMode.value = s_ChosesGenerateMode[0];
            else
                s_GeneratMode.value = s_ChosesGenerateMode[1];
        }

        public void Update()
        {
            if (s_FenceRepitor == null) return;

            s_FenceRepitor.m_GenerateInRealTime = s_GenerateInRealTime.value;

            if (s_GeneratMode.value == LineMode.Points.ToString())
                s_FenceRepitor.m_LineMode = LineMode.Points;
            else
                s_FenceRepitor.m_LineMode = LineMode.Line;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= SelectFenceRepitor;
        }

        public static void SelectFenceRepitor()
        {
            var selectionOnejct = Selection.activeObject;

            if (selectionOnejct is GameObject gameObject)
            {
                if (gameObject.TryGetComponent<FenceRepitor>(out FenceRepitor fenceRepitor1))
                {
                    s_FenceRepitor = fenceRepitor1;
                    SetValue();
                }
                else if (gameObject.transform.parent.transform.parent.TryGetComponent<FenceRepitor>(out FenceRepitor fenceRepitor))
                {
                    s_FenceRepitor = fenceRepitor;
                    SetValue();
                }
            }
        }

        public static void Select()
        {
            if (s_FenceRepitor == null) return;
            Selection.activeGameObject = s_FenceRepitor.gameObject;
        }

        [MenuItem("Tool/World Editor/Fence Generator/Create Point _^g")]
        public static void CreatePoint()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.CreatPoint();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Divide _&d")]
        public static void Divide()
        {
            var point = Selection.activeObject;

            if (point is GameObject gameObject)
            {
                if (gameObject.transform.parent.transform.parent.TryGetComponent<FenceRepitor>(out FenceRepitor fenceRepitor))
                {
                    s_FenceRepitor = fenceRepitor;
                    s_FenceRepitor.Divide();
                }
            }
        }

        [MenuItem("Tool/World Editor/Fence Generator/Covert Point To Line")]
        public static void CovertPointToLine()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.ConverPointsToLine();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Covert Line To Poin")]
        public static void CovertLineToPoint()
        {
            if (s_FenceRepitor == null) return;
        }

        [MenuItem("Tool/World Editor/Fence Generator/Covert Old Points To New")]
        public static void CovertOldPointsToNew()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.ConverOldPointsToNew();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Remove Void Points")]
        public static void RemoveVoidPoints()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.RemoveVoidPoints();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Remove No Valit Points In Line")]
        public static void RemoveNoValitPointsInLine()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.RemoveNoValitPointsInLine();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Remove All Points")]
        public static void RemoveAllPoints()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.RemoveAllPoints();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Remove All Lines")]
        public static void RemoveAllLines()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.RemoveAllLines();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Generate")]
        public static void Generate()
        {
            if (s_FenceRepitor == null) return;

            s_GenerateInRealTime.value = false;
            s_FenceRepitor.GenerateFances();
        }

        [MenuItem("Tool/World Editor/Fence Generator/Generate All Selected")]
        public static void GenerateAllSelected()
        {
            s_GenerateInRealTime.value = false;

            GameObject[] fenceRepitors = Selection.gameObjects;
            
            for (int i = 0; i < fenceRepitors.Length; i++)
            {
                if (fenceRepitors[i].TryGetComponent<FenceRepitor>(out FenceRepitor fenceRepitor))
                    fenceRepitor.GenerateFances();
            }
        }

        [MenuItem("Tool/World Editor/Fence Generator/Destroy All Fences")]
        public static void DestroyAllFences()
        {
            if (s_FenceRepitor == null) return;
            s_FenceRepitor.DestroyAllFance();
        }
    }
}