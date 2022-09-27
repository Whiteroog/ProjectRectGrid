using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseRectTile : MonoBehaviour
    {
        [SerializeField] private TypeTile tileType = TypeTile.Ground;

        private Vector3Int _coordinate;
        public virtual Vector3Int Coordinate
        {
            set => transform.localPosition = _coordinate = value;
            get => _coordinate;
        }

        public TypeTile GetTileType() => tileType;
    }

    public enum TypeTile
    {
        Ground,
        Unit
    }
}