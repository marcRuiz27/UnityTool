using Assets.Engine.Grid;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Editor.MapEditor.GridEditor
{
    internal static class CreateMiGameObject
    {
        [MenuItem("GameObject/Xbraxy/TileMappOverlay", false, 10)]
        static void Create()
        {
            CreateContext();

            GameObject overlayGO = new GameObject("TilemapOverlay");
            Undo.RegisterCreatedObjectUndo(overlayGO, "Create TilemapOverlay");

            overlayGO.transform.SetParent(Selection.activeGameObject.GetComponentInParent<Grid>().transform);
            overlayGO.transform.localPosition = Vector3.zero;

            overlayGO.AddComponent<Tilemap>();
            overlayGO.AddComponent<TilemapRenderer>();
            overlayGO.AddComponent<MyGridManager>();

            Selection.activeGameObject = overlayGO;
        }

        static void CreateContext()
        {
            GameObject context = Selection.activeGameObject;

            Grid grid = null;

            if (context != null)
            {
                grid = context.GetComponentInParent<Grid>();
            }

            GameObject gridGO;

            if(grid==null)
            {
                gridGO = new GameObject("Grid");
                grid = gridGO.AddComponent<Grid>();

                Undo.RegisterCreatedObjectUndo(gridGO, "Create Grid");
                return;
            }

            gridGO = grid.gameObject;
        }

    }
}
