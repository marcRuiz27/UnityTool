

using UnityEngine;

namespace Assets.Engine.Grid
{
    internal class Cell
    {
        public Vector2Int gridPos;
        public Vector3 worldPos;


        public bool isEmpty;
        public int moveCost;
        public int height;

        public GameObject unit;
    }
}
