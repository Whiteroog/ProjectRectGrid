using System;
using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class BaseSquareTileManager : MonoBehaviour
    {
        private Tilemap _tilemap;
        protected GridManager GridManager;

        protected Dictionary<Vector3Int, BaseSquareTile> CachedTiles = new();

        protected virtual void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            GridManager = GetComponentInParent<GridManager>();

            foreach (BaseSquareTile tile in GetComponentsInChildren<BaseSquareTile>())
            {
                CachingTile(tile);
            }
        }

        protected void CachingTile(BaseSquareTile tile)
        {
            Vector3Int coordinateTile = _tilemap.LocalToCell(tile.transform.localPosition);
            tile.SetupTile(this, coordinateTile);
            CachedTiles[coordinateTile] = tile;
        }

        protected void SetTileCoordinate(BaseSquareTile tile, Vector3Int newCoordinate)
        {
            CachedTiles.Remove(tile.Coordinate);
            tile.Coordinate = newCoordinate;
            CachedTiles[newCoordinate] = tile;
        }

        public TTile GetTile<TTile>(Vector3Int coordinate) where TTile : class
        {
            CachedTiles.TryGetValue(coordinate, out BaseSquareTile returnedTile);
            return returnedTile as TTile;
        }
    }
}
