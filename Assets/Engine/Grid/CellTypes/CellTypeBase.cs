

using UnityEngine;

namespace Assets.Engine.Grid.CellTypes
{
    public enum TerrainType
    {
        Basic,
        //Plain,
        //Forest,
        //Mountain,
        //Islando,
        //Swamp,
        //Road,
        Stinky
    }

    public abstract class CellTypeBase : ScriptableObject
    {
        //public TerrainType terrain;
        //public int defaultMovement;
        //public bool blocksVision;
        //public bool walkable;
        public abstract void ApplyAttributesToCell(CellData cell);
    }
}
