using System;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TilemapManager tilemapManager;
        
        [SerializeField] private TileType tileType = TileType.None;

        public Vector3Int Coordinate { private set; get; }

        public void SetupTile(TilemapManager newTilemapManager, Vector3Int newCoordinate)
        {
            tilemapManager = newTilemapManager;
            SetCoordinate(newCoordinate);
        }
        
        public virtual void SetCoordinate(Vector3Int newCoordinate)
        {
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