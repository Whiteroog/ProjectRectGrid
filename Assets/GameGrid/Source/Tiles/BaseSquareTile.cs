using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class BaseSquareTile : MonoBehaviour
    {
        [SerializeField] private BaseSquareTileManager baseSquareTileManager;
        
        [SerializeField] private TileType tileType = TileType.None;

        public virtual Vector3Int Coordinate { protected set; get; }

        public void SetupTile(BaseSquareTileManager newBaseSquareTileManager, Vector3Int newCoordinate)
        {
            baseSquareTileManager = newBaseSquareTileManager;
            Coordinate = newCoordinate;
        }
        
        public TileType GetTileType() => tileType;
    }

    public enum TileType
    {
        None,
        Ground,
        Unit,
        Select
    }
}