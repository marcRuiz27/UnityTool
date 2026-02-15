

namespace Assets.Editor
{
    using UnityEditor;
    using UnityEngine;

    public static class MyGroupBoxUI
    {
        private static GUIStyle _boxStyle;
        private static GUIStyle _headerStyle;

        public static bool DrawFoldoutBox(string title, bool foldout, System.Action content)
        {
            InitStyles();

            EditorGUILayout.BeginVertical(_boxStyle);

            foldout = EditorGUILayout.Foldout(foldout, title, true, _headerStyle);

            if (foldout)
            {
                EditorGUILayout.Space(4);
                EditorGUI.indentLevel++;
                content?.Invoke();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(4);
            }

            EditorGUILayout.EndVertical();

            return foldout;
        }

        public static void DrawBox(string title, System.Action content)
        {
            InitStyles();

            EditorGUILayout.BeginVertical(_boxStyle);
            EditorGUILayout.LabelField(title, _headerStyle);

            EditorGUILayout.Space(4);
            EditorGUI.indentLevel++;
            content?.Invoke();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(4);

            EditorGUILayout.EndVertical();
        }

        private static void InitStyles()
        {
            if (_boxStyle == null)
            {
                _boxStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(10, 10, 8, 8)
                };
            }

            if (_headerStyle == null)
            {
                _headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 12
                };
            }
        }
    }

    internal class LevelWindowEditor : EditorWindow
    {

        float _cellSize;
        int _gridHeight;
        [MenuItem("Xbraxy/External Gid Level Editor/Level Window")]
        public static void Open()
        {
            GetWindow<LevelWindowEditor>("LevelWindowEditor");
        }
        private bool _showGridSettings = true;

        private void OnGUI()
        {
            _showGridSettings = MyGroupBoxUI.DrawFoldoutBox(
                "Grid Settings",
                _showGridSettings,
                () =>
                {
                    _cellSize = EditorGUILayout.FloatField("Cell Size", _cellSize);
                    _gridHeight = EditorGUILayout.IntField("Grid Height", _gridHeight);
                });
        }
    }
}
