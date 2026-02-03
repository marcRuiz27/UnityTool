
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Engine.Grid;
namespace Assets.Editor
{
    [CustomEditor(typeof(Assets.Engine.Grid.MyGridBuilder))]
    public class MyGridBuilderEditor : UnityEditor.Editor
    {
        private MyGridBuilder _gridBuilder;

        private SerializedProperty _tilesArray;

        private SerializedProperty _outputGrid;


        public void OnEnable()
        {
            if (target == null)
                return;

            _gridBuilder = (MyGridBuilder)target;

            if (_gridBuilder != null)
                return;

        }

        private void ReferenceObjects()
        {
            _gridBuilder = (MyGridBuilder)target;

            if (_gridBuilder != null)
                return;

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            //GUILayout.Space(10);
            //if (GUILayout.Button("Collect Tiles From Tilemap"))
            //{
            //    Undo.RecordObject(_gridBuilder, "Collect Tiles");
            //    BrowseTilesFromTileMap();
            //    tilesInstanciated = true;
            //    EditorUtility.SetDirty(_gridBuilder);
            //}

            GUILayout.Space(10);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            if (GUILayout.Button("Bake Logical Grid"))
            {              
                
                    //Debug.LogWarning("Unable Action. First store the tile list");
                    //return;

                Undo.RecordObject(_gridBuilder, "Collect Tiles");
                _gridBuilder.Bake();
                EditorUtility.SetDirty(_gridBuilder.GetGrid);
            }

            serializedObject.ApplyModifiedProperties();
        }

        //public void BrowseTilesFromTileMap()
        //{
        //    Tilemap[] currentTilemaps = _gridBuilder.GetTilemaps;
        //    if (currentTilemaps == null)
        //    {
        //        Debug.LogWarning("No Tilemap assigned");
        //        return;
        //    }

        //    HashSet<TileBase> foundTiles = new HashSet<TileBase>();

        //    BoundsInt bounds = _gridBuilder.GetTilemaps.cellBounds;

        //    foreach (Vector3Int pos in bounds.allPositionsWithin)
        //    {
        //        TileBase tile = currentTilemaps.GetTile(pos);
        //        if (tile != null)
        //        {
        //            foundTiles.Add(tile);
        //        }
        //    }

        //    //m_builder.TileBases = new TileBase[foundTiles.Count];
        //    //foundTiles.CopyTo(m_builder.TileBases);
        //    _gridBuilder.SetTileBases(foundTiles.ToArray());
        //    Debug.Log($"Collected {_gridBuilder.TileBases.Count} unique tiles from Tilemap.");
        //}

    }
}
