
using Assets.Engine.Grid.CellTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Engine.Grid
{
    [CreateAssetMenu(menuName = "Plugin/Grid/CellTypes/Stinky")]
    public class StinkyCellType : CellTypeBase
    {
        public TerrainType terrain = TerrainType.Stinky;
        public int defaultMovement = 2;
        public bool walkable = true;

        //public override void ApplyAttributesToCell(CellData cell)
        //{
        //    cell.TerrainType = terrain;
        //    cell.MovementCost = defaultMovement;
        //    cell.Walkable = walkable;
        //}

        public override bool GetIsWalkable()
        {
            return walkable;
        }

        public override int GetMovementCost()
        {
            return defaultMovement;
        }

        public override TerrainType GetTerrainType()
        {
            return terrain;
        }


    }
}
