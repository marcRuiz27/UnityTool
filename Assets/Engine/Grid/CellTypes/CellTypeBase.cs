

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Engine.Grid.CellTypes
{
    public enum TerrainType
    {
        None,
        Basic,
        //Plain,
        //Forest,
        //Mountain,
        //Islando,
        //Swamp,
        //Road,
        Stinky
    }

    /// <summary>
    /// WARNING: INHETIRANCE CLASSES MUST NOT SHARE TILES ASSETS
    /// TOOL REQUIRED
    /// </summary>
    public abstract class CellTypeBase : ScriptableObject
    {
        [SerializeField]
        private TileBase[] tiles;
      // public abstract void ApplyAttributesToCell(CellData cell);
        public bool HasSpecificTileName(string tilename)
        {
            if (tiles == null || tiles.Length == 0)
                return false;

            for (int i = 0; i < tiles.Length; i++)
            {
                return tiles[i].name == tilename;
            }

            return false;
        }

        public abstract int GetMovementCost();
        public abstract TerrainType GetTerrainType();
        public abstract bool GetIsWalkable();
    }
}
