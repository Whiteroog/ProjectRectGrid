using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseSquareTile : MonoBehaviour
    {
        [SerializeField] private TileType tileType = TileType.None;

        private BaseSquareTileManager _tileManager;

        private Vector3Int _coordinate;
        public virtual Vector3Int Coordinate
        {
            set => transform.position = _coordinate = value;
            get => _coordinate;
        }

        public void SetupTile(BaseSquareTileManager newBaseSquareTileManager, Vector3Int newCoordinate)
        {
            _tileManager = newBaseSquareTileManager;
            Coordinate = newCoordinate;
        }

        public TileType GetTileType() => tileType;
        public TManager GetTileManager<TManager>() where TManager : class => _tileManager as TManager;
    }

    public enum TileType
    {
        None,
        Ground,
        Unit
    }
}