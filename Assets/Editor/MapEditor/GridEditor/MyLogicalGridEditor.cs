using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Editor.MapEditor.GridEditor
{
    [CustomEditor(typeof(Assets.Engine.Grid.MyGridManager))]    
    public class MyLogicalGridEditor : UnityEditor.Editor
    {
        
        public void OnEnable()
        {
            if (target == null)
            {
                Debug.LogWarning("The target gameObject is null");
                return;
            }
        }

        //public override void OnInspectorGUI()
        //{
        //    DrawDefaultInspector();

        //    EditorGUILayout.LabelField("Grid Size", EditorStyles.boldLabel);

        //}
    }
}
