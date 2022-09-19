using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = GameGrid.Source.Tiles.Tile;

namespace GameGrid.Source.Managers
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        
        private Dictionary<Vector3Int, Tile> _cachedTiles = new();

        private void Awake()
        {
            Tile[] tiles = GetComponentsInChildren<Tile>();

            foreach (Tile tile in tiles)
            {
                Vector3Int newCoordinateForTile = tilemap.LocalToCell(tile.transform.localPosition);
                tile.SetCoordinate(newCoordinateForTile);
                
                _cachedTiles[newCoordinateForTile] = tile;
            }
        }
    }
}
