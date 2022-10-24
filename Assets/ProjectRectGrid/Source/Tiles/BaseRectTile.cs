using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseRectTile : MonoBehaviour
    {
        public virtual Vector3Int Coordinate { set; get; }
    }
}