using System;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.System;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class BaseSquareTileManager : MonoBehaviour
    {
        private Tilemap _tilemap;
        protected GridSystem GridSystem;

        private Dictionary<Vector3Int, BaseSquareTile> _cachedTiles = new();

        protected virtual void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            GridSystem = GetComponentInParent<GridSystem>();
            
            BaseSquareTile[] tiles = GetComponentsInChildren<BaseSquareTile>(); // can find derived class

            foreach (BaseSquareTile tile in tiles)
            {
                CachingTile(tile);
            }
        }

        protected void CachingTile(BaseSquareTile tile)
        {
            Vector3Int coordinatePlacedTile = _tilemap.LocalToCell(tile.transform.localPosition);
            tile.SetupTile(this, coordinatePlacedTile);
            _cachedTiles[coordinatePlacedTile] = tile;
        }

        protected void SetTileCoordinate(BaseSquareTile tile, Vector3Int newCoordinate)
        {
            _cachedTiles.Remove(tile.Coordinate);
            tile.Coordinate = newCoordinate;
            _cachedTiles[newCoordinate] = tile;
        }

        public TTile GetTile<TTile>(Vector3Int coordinate) where TTile : class
        {
            _cachedTiles.TryGetValue(coordinate, out BaseSquareTile returnedTile);
            return returnedTile as TTile;
        }
    }
}
