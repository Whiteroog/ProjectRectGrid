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
        
        protected Dictionary<Vector3Int, BaseSquareTile> CachedTiles = new();

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

        public void CachingTile(BaseSquareTile tile)
        {
            Vector3Int coordinatePlacedTile = _tilemap.LocalToCell(tile.transform.localPosition);
            tile.SetupTile(this, coordinatePlacedTile);
            CachedTiles[coordinatePlacedTile] = tile;
        }
        
        public void SetTileCoordinate(BaseSquareTile tile, Vector3Int newCoordinate)
        {
            CachedTiles.Remove(tile.Coordinate);
            tile.Coordinate = newCoordinate;
            CachedTiles[newCoordinate] = tile;
        }

        public BaseSquareTile GetTile(Vector3Int coordinate)
        {
            CachedTiles.TryGetValue(coordinate, out BaseSquareTile returnedTile);
            return returnedTile;
        }
    }
}
