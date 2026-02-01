
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Engine.Grid
{
    /// <summary>
    /// Create data grid En runtime usare MyGridManager + LogicalGridAsset, nunca el GridBuilder.
    /// </summary>
    public class MyGridBuilder : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField]
        private UnityEngine.Grid _grid;
        [SerializeField]
        private Tilemap _basicTilemap;
  
        [HideInInspector]
        [SerializeField]
        private TileBase[] _tilesBase;
        
        [SerializeField]
        private CellTypes.BasicCellType cellsTypes;

        [Header("Output")]
        [SerializeField]
        private LogicalGridAsset _outputGrid;

        public UnityEngine.Grid GetGrid => _grid;
        public Tilemap GetSourceTilemap => _basicTilemap;

        public IReadOnlyList<TileBase> TileBases => _tilesBase;
        public void SetTileBases(TileBase[] tilesBase)
        {
            _tilesBase = tilesBase;
        }

        //public TileBase[] elevationTiles;
        //public BasicCellType elevationTypes;



        public void Bake()
        {
            _basicTilemap.CompressBounds();

            BoundsInt bounds = _basicTilemap.cellBounds;

            int boundWidth = bounds.size.x;
            int boundHeight = bounds.size.y;

            _outputGrid.SetSizeLogicalGrid(boundWidth, boundHeight);
            _outputGrid._cells = new CellData[boundWidth * boundHeight];

            for (int y = 0; y < boundHeight; y++)
            {
                for (int x = 0; x < boundWidth; x++)
                {
                    Vector3Int cellPos = new Vector3Int(
                        bounds.xMin + x,
                        bounds.yMin + y,
                        0
                    );

                    if (!_basicTilemap.HasTile(cellPos))
                        continue;

                    CellData cell = new CellData();
                    cell.gridPos = new Vector2Int(x, y);

                    //en isometrico 
                    cell.worldPos = _basicTilemap.CellToWorld(cellPos) + _grid.cellSize * 0.5f;

                    ApplyGround(cell, cellPos);
                    //ApplyElevation(cell, cellPos);

                    _outputGrid._cells[y * boundWidth + x] = cell;
                }
            }

            Debug.Log("Logical Grid baked successfully");
        }

        void ApplyGround(CellData cell, Vector3Int pos)
        {
            TileBase tile = _basicTilemap.GetTile(pos);
            for (int i = 0; i < _tilesBase.Length; i++)
            {
                if (_tilesBase[i] == tile)
                {
                    cell.terrainType = cellsTypes.terrain;
                    cell.movementCost = cellsTypes.defaultMovement;
                    cell.walkable = cellsTypes.walkable;
                    return;
                }
            }
        }


        //void ApplyElevation(CellData cell, Vector3Int pos)
        //{
        //    if (!StinkyTileMap.HasTile(pos)) return;

        //    TileBase tile = StinkyTileMap.GetTile(pos);
        //    for (int i = 0; i < elevationTiles.Length; i++)
        //    {
        //        if (elevationTiles[i] == tile)
        //        {
        //            cell.height = elevationTypes.height;
        //            return;
        //        }
        //    }
        //}
    }
}
