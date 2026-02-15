using Assets.Engine.Grid;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.MapEditor.GridEditor
{
    public class GridManagerWindowCreator : UnityEditor.Editor
    {

        [MenuItem("Xbraxy/GameObject/Mi GameObject con Datos", false, 10)]
        static void Create()
        {
            GameObject go = new GameObject("MyGridManager");
            go.AddComponent<MyGridManager>();

            Selection.activeGameObject = go;
        }
    }
}
