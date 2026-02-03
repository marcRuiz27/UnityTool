using System;
using UnityEngine;

namespace Assets.Engine.Grid
{
    /// <summary>
    /// This class represents the each individual map position during runtime
    /// </summary>
    [Serializable]
    public sealed class CellData
    {
        private Vector3 _worldPos;
        private Vector2Int _gridPos;

        private CellTypes.TerrainType _terrainType = CellTypes.TerrainType.None;
        private int _movementCost = -1;
        private int _heightLayer = -1;
        private bool _walkable = false;

        public Vector2Int GridPos
        {
            get { return _gridPos; }
            set { _gridPos = value; }
        }
        public Vector3 WorldPos
        {
            get { return _worldPos; }
            set { _worldPos = value; }
        }
        public CellTypes.TerrainType TerrainType
        {
            get {  return _terrainType; }
            set { _terrainType = value; }
        }
        public int MovementCost
        {
            get { return _movementCost; }
            set { _movementCost = value; }
        }
        public int HeightLayer
        {
            get { return _heightLayer; }
            set { _heightLayer = value; }
        }
        public bool Walkable
        {
            get { return _walkable; }
            set { _walkable = value; }
        }
    }

}
