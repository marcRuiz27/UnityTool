
using Assets.Engine.Grid.CellTypes;
using UnityEngine;

namespace Assets.Engine.Grid
{
    [CreateAssetMenu(menuName = "Plugin/Grid/CellTypes/Stinky")]
    public class StinkyCellType : CellTypeBase
    {
        public TerrainType terrain = TerrainType.Stinky;
        public int defaultMovement = 2;
        public bool walkable = true;

        public override void ApplyAttributesToCell(CellData cell)
        {
            cell.terrainType = terrain;
            cell.movementCost = defaultMovement;
            cell.walkable = walkable;
        }
    }
}
