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
        
        private Dictionary<Vector3Int, BaseSquareTile> _cachedTiles = new();
        
        private void Awake()
        {
            BaseSquareTile[] tiles = GetComponentsInChildren<BaseSquareTile>(); // can find derived class

            foreach (BaseSquareTile tile in tiles)
            {
                Vector3Int coordinatePlacedTile = tilemap.LocalToCell(tile.transform.localPosition);
                tile.SetupTile(this, coordinatePlacedTile);
                
                _cachedTiles[coordinatePlacedTile] = tile;
            }
        }
    }
}
