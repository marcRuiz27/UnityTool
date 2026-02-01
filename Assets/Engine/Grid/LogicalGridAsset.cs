
using System;
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

        public int Heigt => _height;
        public int Width => _width;
        public CellData[] _cells;



        public void SetSizeLogicalGrid(int width, int height)
        {
            _width = width;
            _height = height;
            Debug.Log($"Size of the lvl: Width={_width} Height={_height}");
        }

        public CellData GetCell(Vector2Int pos)
        {
            int index = pos.y * Width + pos.x;
            return _cells[index];
        }
    }
}
