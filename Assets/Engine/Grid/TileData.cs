using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Engine.Grid
{
    [CreateAssetMenu(menuName = "Plugin/Tiles/TileData")]
    public class TileData : ScriptableObject
    {
        /// <summary>
        /// Maybe it could be replaced by type Tile
        /// </summary>
        public TileBase[] tiles;

        public bool isEmpty;
        public int moveCost;
   //     public int height;
        public bool blocksVision;

    }
}
