using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseSquareTile : MonoBehaviour
    {
        [SerializeField] private BaseSquareTileManager baseSquareTileManager;
        
        [SerializeField] private TileType tileType = TileType.None;

        private Vector3Int _coordinate;
        public virtual Vector3Int Coordinate
        {
            set
            {
                transform.position = value;
                _coordinate = value;
                baseSquareTileManager.UpdateCoordinateInCache(this, value);
            }
            get => _coordinate;
        }

        public void SetupTile(BaseSquareTileManager newBaseSquareTileManager, Vector3Int newCoordinate)
        {
            baseSquareTileManager = newBaseSquareTileManager;
            Coordinate = newCoordinate;
        }
        
        public TileType GetTileType() => tileType;

        public BaseSquareTileManager GetTileManager() => baseSquareTileManager;
    }

    public enum TileType
    {
        None,
        Ground,
        Unit,
        Select
    }
}