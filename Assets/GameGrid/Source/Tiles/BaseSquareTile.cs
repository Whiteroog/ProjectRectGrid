using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseSquareTile : MonoBehaviour
    {
        [SerializeField] private TileType tileType = TileType.Ground;

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
        public TManager GetTileManager<TManager>() where TManager : class => _baseSquareTileManager as TManager;
    }

    public enum TileType
    {
        Ground,
        UnitPlayer,
        UnitEnemy,
        Select,
        PointWay
    }
}