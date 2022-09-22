using System;
using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class BaseSquareTileManager : MonoBehaviour
    {
        [SerializeField] protected Tilemap tilemap;
        
        protected Dictionary<Vector3Int, BaseSquareTile> cachedHavingTiles = new();

        protected virtual void Awake()
        {
            BaseSquareTile[] tiles = GetComponentsInChildren<BaseSquareTile>(); // can find derived class

            foreach (BaseSquareTile tile in tiles)
            {
                Vector3Int coordinatePlacedTile = tilemap.LocalToCell(tile.transform.localPosition);
                tile.SetupTile(this, coordinatePlacedTile);
                
                cachedHavingTiles[coordinatePlacedTile] = tile;
            }
        }

        public void UpdateCoordinateInCache(BaseSquareTile updatingTile, Vector3Int newCoordinate)
        {
            if (cachedHavingTiles.Remove(updatingTile.Coordinate))
            {
                cachedHavingTiles[newCoordinate] = updatingTile;
            }
        }
    }
}
