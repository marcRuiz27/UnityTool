
using UnityEngine;

namespace Assets.Engine.Grid.CellTypes
{

    [CreateAssetMenu(menuName = "Plugin/Grid/CellTypes/Basic")]
    public class BasicCellType : CellTypeBase
    {
        public TerrainType terrain = TerrainType.Basic;
        public int defaultMovement = 1;
        public bool walkable = true;

        public override void ApplyAttributesToCell(CellData cell)
        {
            cell.terrainType = terrain;
            cell.movementCost = defaultMovement;
            cell.walkable = walkable;
        }
    }




}
