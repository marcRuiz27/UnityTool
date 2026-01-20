using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(MyComponent))]
    public class MiComponenteEditor : UnityEditor.Editor
    {
        SerializedProperty velocidad;
        SerializedProperty vida;
        SerializedProperty battleCry;
        SerializedProperty enumPlayer;

        public void OnEnable()
        {
            if (target == null)
                return;
            //if (serializedObject == null || serializedObject.targetObject == null)
            //    return;
            velocidad = serializedObject.FindProperty("_velocidad");
            vida = serializedObject.FindProperty("_vida");
            // atkRange = serializedObject.FindProperty("_range");
            battleCry = serializedObject.FindProperty("_battleCry");
            enumPlayer = serializedObject.FindProperty("_enum");

        }

        public override void OnInspectorGUI()
        {
            //if (serializedObject == null || serializedObject.targetObject == null)
            //    return;

            if (velocidad == null || vida == null || battleCry == null || enumPlayer == null)
            {
                EditorGUILayout.HelpBox(
                    "No se encontraron las propiedades serializadas. Revisa los nombres.",
                    MessageType.Error);
                return;
            }

            serializedObject.Update();

            //Esto dibuja en el inspector de unity en loop
            EditorGUILayout.LabelField("Inspector personalizado", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(velocidad);
            EditorGUILayout.PropertyField(vida);
            EditorGUILayout.PropertyField (battleCry);
            EditorGUILayout.PropertyField (enumPlayer);

            if (GUILayout.Button("Resetear valores"))
            {
                enumPlayer.enumValueFlag = 0;
                velocidad.floatValue = 5f;
                vida.intValue = 100;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
