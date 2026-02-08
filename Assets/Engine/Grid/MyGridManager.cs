

using UnityEngine;

namespace Assets.Engine.Grid
{
    public class MyGridManager : MonoBehaviour
    {
        public LogicalGridAsset levelGrid;

        //private CellData[] runtimeCells;

        //void Awake()
        //{
        //    runtimeCells = new CellData[levelGrid.cells.Length];

        //    for (int i = 0; i < levelGrid.cells.Length; i++)
        //    {
        //        runtimeCells[i] = JsonUtility.FromJson<CellData>(
        //            JsonUtility.ToJson(levelGrid._cells[i])
        //        );
        //    }
        //}

        //public CellData GetCell(Vector2Int pos)
        //{
        //    int index = pos.y * levelGrid.Width + pos.x;
        //    return runtimeCells[index];
        //}
    }
}
