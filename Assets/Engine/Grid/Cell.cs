

using UnityEngine;

namespace Assets.Engine.Grid
{
    internal class Cell
    {
        [SerializeField]
        public Vector2Int gridPos;
        [SerializeField]
        public Vector3 worldPos;
        

        public bool isEmpty;
        public int moveCost;
        public int height;

        public GameObject unit;
    }
}
