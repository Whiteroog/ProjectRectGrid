using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseRectTile : MonoBehaviour
    {
        [SerializeField] private TypeTile tileType = TypeTile.None;

        private Vector3Int _coordinate;
        public virtual Vector3Int Coordinate
        {
            set => transform.position = _coordinate = value;
            get => _coordinate;
        }

        public TypeTile GetTileType() => tileType;
    }

    public enum TypeTile
    {
        None,
        Ground,
        Unit
    }
}