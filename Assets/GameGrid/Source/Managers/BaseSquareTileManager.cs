using System;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class BaseSquareTileManager : MonoBehaviour
    {
        protected Tilemap Tilemap;
        
        protected Dictionary<Vector3Int, BaseSquareTile> CachedTiles = new();

        protected virtual void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
            
            BaseSquareTile[] tiles = GetComponentsInChildren<BaseSquareTile>(); // can find derived class

            foreach (BaseSquareTile tile in tiles)
            {
                CachingTile(tile);
            }
        }

        public void CachingTile(BaseSquareTile tile)
        {
            Vector3Int coordinatePlacedTile = Tilemap.LocalToCell(tile.transform.localPosition);
            tile.SetupTile(this, coordinatePlacedTile);
        }

        public void UpdateCoordinateInCache(BaseSquareTile updatingTile, Vector3Int oldCoordinate)
        {
            CachedTiles.Remove(oldCoordinate);
            CachedTiles[updatingTile.Coordinate] = updatingTile;
        }

        public BaseSquareTile GetTile(Vector3Int coordinate)
        {
            CachedTiles.TryGetValue(coordinate, out BaseSquareTile returnedTile);
            return returnedTile;
        }
    }
}
