
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
    [CreateAssetMenu(menuName = "Xbraxy/ScriptableObjects/Grid/CellType")]
    public class CellTypeSO : ScriptableObject
    {
        [SerializeField]
        private TileBase _editorOverlayTile;

        [SerializeField]
        private TerrainType _terrainType;

        [SerializeField]
        private int _moveCost;

        [SerializeField]
        private bool _walkable;

        [SerializeField]
        private int _height = 0;
        //public bool HasSpecificTileName(string tilename)
        //{
        //    if (tiles == null || tiles.Length == 0)
        //        return false;

        //    for (int i = 0; i < tiles.Length; i++)
        //    {
        //        return tiles[i].name == tilename;
        //    }

        //    return false;
        //}

        public TileBase OverlayTile => _editorOverlayTile;
        public TerrainType TerrainType => _terrainType;
        public int MoveCost => _moveCost;
        public bool Walkable => _walkable;
        public int Height => _height;


        //public override void ApplyAttributesToCell(CellData cell)
        //{
        //    cell.TerrainType = terrain;
        //    cell.MovementCost = defaultMovement;
        //    cell.Walkable = walkable;
        //}
    }




}
