﻿using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class GroundTilesManager : BaseSquareTileManager
    {
        public List<GroundTile> GetNeighboursFor(Vector3Int coordinate)
        {
            List<GroundTile> neighboursTileCoordinates = new List<GroundTile>();

            foreach (Vector3Int direction in _directionsSquareTile)
            {
                if(GetTile<GroundTile>(coordinate + direction) is null)
                    continue;
                
                neighboursTileCoordinates.Add(GetTile<GroundTile>(coordinate + direction));
            }

            return neighboursTileCoordinates;
        }

        private List<Vector3Int> _directionsSquareTile = new()
        {
                            new Vector3Int(0, 1, 0),
            new Vector3Int(-1, 0, 0),     new Vector3Int(1, 0, 0),
                            new Vector3Int(0, -1, 0)
        };
    }
}