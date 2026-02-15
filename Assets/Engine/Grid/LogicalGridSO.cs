
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Engine.Grid
{
    /// <summary>
    /// This class contains all the CellsData
    /// </summary>
    [CreateAssetMenu(menuName = "Xbraxy/ScriptableObjects/Grid/Logical Grid",
        fileName ="LvlGrid")]
    public class LogicalGridSO : ScriptableObject
    {
        private Vector2Int _size;
        private CellData[] _cells;

        public int SixeX => _size.x;
        public int SizeY => _size.y;

        public IReadOnlyList<CellData> GetCellsArray => _cells;
        public CellData GetCellByIdx(int idx) => _cells[idx]?? throw new IndexOutOfRangeException($"Idx {idx} out of hte array {_cells.Length}");
        public CellData GetCellByGridPos(Vector2Int pos)
        {
            int index = pos.y * SizeY + pos.x;
            return _cells[index];
        }

        public void SetCell(int idx, CellData cell)
        {
            _cells[idx] = cell;
        }
        public void SetSizeLogicalGrid(int sizeX, int sizeY)
        {
            _size = new Vector2Int(sizeX, sizeY);
            _cells = new CellData[sizeX * sizeY];
            Debug.Log($"LvlGrid: Width={sizeX} Height={sizeY}");
        }
        public LogicalGridSO(CellData[] cells)
        {
            _cells = cells.ToArray();

            int minX, minY, maxX, maxY = 0;
            
            foreach (CellData cell in _cells)
            {

            }

        }

    }
}
