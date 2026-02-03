
using UnityEngine.Tilemaps;
using UnityEngine;
using Assets.Engine.Grid.CellTypes;

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
        private Tilemap[] _listTilemaps;

        [SerializeField]
        private CellTypeBase[] _cellTypes;


        [Header("Output")]
        [SerializeField]
        private LogicalGridAsset _outputLvlGrid;





        public UnityEngine.Grid GetGrid => _grid;
        public Tilemap[] GetTilemaps => _listTilemaps;


        public void DefineGridLvlSize()
        {
            int boundWidth = 0;
            int boundHeight = 0;

            var _boundsGrid = new BoundsInt();

            if (_listTilemaps == null)
            {
                Debug.LogError("No tilemap objects found");
                return;
            }

            Debug.Log($"ListTileMap length: {_listTilemaps.Length}");
            //fins the largest tilemap in both axis
            for (int i = 0; i < _listTilemaps.Length; i++)
            {
                _listTilemaps[i].CompressBounds();

                _boundsGrid = _listTilemaps[i].cellBounds;

                if (boundWidth < _boundsGrid.size.x)
                    boundWidth = _boundsGrid.size.x;

                if (boundHeight < _boundsGrid.size.y)
                    boundHeight = _boundsGrid.size.y;
            }

            _outputLvlGrid.SetSizeLogicalGrid(boundWidth, boundHeight);
        }

        public void Bake()
        {
            DefineGridLvlSize();

            foreach (Tilemap tilemap in _listTilemaps)
            {
                for (int y = 0; y < tilemap.cellBounds.size.x; y++)
                {
                    for (int x = 0; x < tilemap.cellBounds.size.y; x++)
                    {
                        Vector3Int cellPos = new Vector3Int(tilemap.cellBounds.xMin + x, tilemap.cellBounds.yMin + y, 0);

                        if (!tilemap.HasTile(cellPos))
                            continue;

                        //if +1 cells exists in diferents tilemaps in the same position it will be priorized the one's with a highest layer 
                        var idxCell = y * tilemap.cellBounds.size.x + x;
                        var heightCell = tilemap.cellBounds.size.y - idxCell;
                        return;
                        //Debug.Log($"{tilemap.gameObject.GetComponent<TilemapRenderer>().sortingLayerID}");
                        if (tilemap.gameObject.GetComponent<TilemapRenderer>().sortingLayerID < _outputLvlGrid.GetCellByIdx(idxCell).HeightLayer)
                        {
                            CellData cell = new CellData();
                            cell.GridPos = new Vector2Int(x, y);

                            var tilebase = tilemap.GetTile(cellPos);
                            cell.WorldPos = tilemap.CellToWorld(cellPos) + _grid.cellSize * 0.5f;

                            //ApplyTileProperties(cell, cellPos);
                            ApplyTileProperties(cell, tilebase.name);
     
                            _outputLvlGrid.SetCell(idxCell, cell);
                        }

                    }
                }

            }

            Debug.Log("Logical Grid baked successfully");
        }


        private void ApplyTileProperties(CellData cell, string tilemapName)//CellData cell, Vector3Int pos)
        {
            //TileBase tile = _listTilemaps.GetTile(pos);
            //for (int i = 0; i < _tileTypes.Length; i++)
            //{
            //    if (_tileTypes[i] == tile)
            //    {
            //        cell.TerrainType = _cellTypes.terrain;
            //        cell.MovementCost = _cellTypes.defaultMovement;
            //        cell.Walkable = _cellTypes.walkable;
            //        return;
            //    }
            //}
            foreach(CellTypeBase cellType in _cellTypes)
            {
                if (cellType == null)
                {
                    return;
                }

                if(cellType.HasSpecificTileName(tilemapName))
                {
                    cell.TerrainType = cellType.GetTerrainType();
                    cell.MovementCost = cellType.GetMovementCost();
                    cell.Walkable = cellType.GetIsWalkable();
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
