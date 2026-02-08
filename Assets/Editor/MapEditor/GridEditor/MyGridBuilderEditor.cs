
using UnityEditor;
using Assets.Engine.Grid;
namespace Assets.Editor
{
    [CustomEditor(typeof(Assets.Engine.Grid.MyGridBuilder))]
    public class MyGridBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            MyGridManager comp = (MyGridManager)target;

            if (comp != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Datos Internos", EditorStyles.boldLabel);

                UnityEditor.Editor editorSO = CreateEditor(comp.levelGrid);
                editorSO.OnInspectorGUI();
            }
        }

    }
}
