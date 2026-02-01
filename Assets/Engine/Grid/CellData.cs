using System;
using UnityEngine;

namespace Assets.Engine.Grid
{
    /// <summary>
    /// This class represents the each individual map position during runtime
    /// </summary>
    [Serializable]
    public class CellData
    {
        public Vector2Int gridPos;
        public Vector3 worldPos;

        public CellTypes.TerrainType terrainType;   // Plains, Forest, Road…
        public int movementCost;
        public bool walkable;
    }

}
