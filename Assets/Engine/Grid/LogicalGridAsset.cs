
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Engine.Grid
{
    /// <summary>
    /// This class contains all the CellsData
    /// </summary>
    [CreateAssetMenu(menuName = "Plugin/Grid/Logical Grid")]
    public class LogicalGridAsset : ScriptableObject
    {
        private int _width = 0;
        private int _height = 0;
        private CellData[] _cells;

        public int Heigt => _height;
        public int Width => _width;

        public IReadOnlyList<CellData> GetCellsArray => _cells;
        public CellData GetCellByIdx(int idx) => _cells[idx]?? throw new IndexOutOfRangeException($"Idx {idx} out of hte array {_cells.Length}");
        public CellData GetCellByGridPos(Vector2Int pos)
        {
            int index = pos.y * Width + pos.x;
            return _cells[index];
        }

        public void SetCell(int idx, CellData cell)
        {
            _cells[idx] = cell;
        }
        public void SetSizeLogicalGrid(int width, int height)
        {
            _width = width;
            _height = height;
            _cells = new CellData[width * height];
            Debug.Log($"LvlGrid: Width={_width} Height={_height}");
        }


    }
}
