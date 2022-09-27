using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseRectTile : MonoBehaviour
    {
        private Vector3Int _coordinate;
        public virtual Vector3Int Coordinate
        {
            set => transform.localPosition = _coordinate = value;
            get => _coordinate;
        }
    }
}