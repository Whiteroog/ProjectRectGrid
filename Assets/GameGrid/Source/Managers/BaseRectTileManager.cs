using System;
using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class BaseRectTileManager : MonoBehaviour
    {
        private Tilemap _tilemap;
        protected GridManager GridManager;

        protected Dictionary<Vector3Int, BaseRectTile> CachedTiles = new();

        protected virtual void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            GridManager = GetComponentInParent<GridManager>();

            foreach (BaseRectTile tile in GetComponentsInChildren<BaseRectTile>())
            {
                CachingTile(tile);
            }
        }

        protected void CachingTile(BaseRectTile tile)
        {
            Vector3Int coordinateTile = _tilemap.LocalToCell(tile.transform.localPosition);
            tile.SetupTile(this, coordinateTile);
            CachedTiles[coordinateTile] = tile;
        }

        protected void SetTileCoordinate(BaseRectTile tile, Vector3Int newCoordinate)
        {
            CachedTiles.Remove(tile.Coordinate);
            tile.Coordinate = newCoordinate;
            CachedTiles[newCoordinate] = tile;
        }

        public TTile GetTile<TTile>(Vector3Int coordinate) where TTile : class
        {
            CachedTiles.TryGetValue(coordinate, out BaseRectTile returnedTile);
            return returnedTile as TTile;
        }
    }
}
