
using UnityEngine;

namespace Assets.Engine.Grid.CellTypes
{

    [CreateAssetMenu(menuName = "Plugin/Grid/CellTypes/Basic")]
    public class BasicCellType : CellTypeBase
    {
        public TerrainType terrain = TerrainType.Basic;
        public int defaultMovement = 1;
        public bool walkable = true;

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

        //public override void ApplyAttributesToCell(CellData cell)
        //{
        //    cell.TerrainType = terrain;
        //    cell.MovementCost = defaultMovement;
        //    cell.Walkable = walkable;
        //}
    }




}
