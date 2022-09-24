using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseSquareTile : MonoBehaviour
    {
        [SerializeField] private TileType tileType = TileType.None;

        private BaseSquareTileManager _baseSquareTileManager;

        private Vector3Int _coordinate;

        public virtual Vector3Int Coordinate
        {
            set => transform.position = _coordinate = value;
            get => _coordinate;
        }

        public void SetupTile(BaseSquareTileManager newBaseSquareTileManager, Vector3Int newCoordinate)
        {
            _baseSquareTileManager = newBaseSquareTileManager;
            Coordinate = newCoordinate;
        }

        public TileType GetTileType() => tileType;
        public BaseSquareTileManager GetTileManager() => _baseSquareTileManager;
    }

    public enum TileType
    {
        None,
        Ground,
        UnitPlayer,
        UnitEnemy,
        Select,
        PointWay
    }
}