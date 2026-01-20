using UnityEngine;
using UnityEditor;
using System.ComponentModel;

public class MyToolEditor : EditorWindow
{
    float velocidadPorDefecto = 10f;
    int vidaPorDefecto = 200;

    [MenuItem("Tools/Mi Herramienta")]
    public static void Abrir()
    {
        //Esto hace que cuando se seleccione del cmbBox la opcion definida suceda algo
        GetWindow<MyToolEditor>("Nombre Ventana Tool");
    }

    public void OnGUI()
    {
        GUILayout.Label("Label EJemplo: Herramienta nativa Unity", EditorStyles.boldLabel);

        velocidadPorDefecto = EditorGUILayout.FloatField("Velocidad por defecto", velocidadPorDefecto);

        vidaPorDefecto = EditorGUILayout.IntField("Vida por defecto", vidaPorDefecto);

        if (GUILayout.Button("Añadir MiComponente al objeto seleccionado"))
        {
            GameObject obj = Selection.activeGameObject;

            if (obj == null)
            {
                Debug.LogWarning("Selecciona un GameObject primero");
                return;
            }
            MyComponent comp = obj.GetComponent<MyComponent>();

            if (comp == null)
            {
                comp = Undo.AddComponent<MyComponent>(obj);
            }
            //Esto es para runtime
            //Undo.RecordObject(comp, "Configurar MiComponente");
            //comp.velocidad = velocidadPorDefecto;
            //comp.vida = vidaPorDefecto;
            SerializedObject so = new SerializedObject(comp);

            SerializedProperty velProp = so.FindProperty("velocidad");
            SerializedProperty vidaProp = so.FindProperty("vida");

            so.Update();

            velProp.floatValue = velocidadPorDefecto;
            vidaProp.intValue = vidaPorDefecto;

            so.ApplyModifiedProperties();

        }
    }
}
