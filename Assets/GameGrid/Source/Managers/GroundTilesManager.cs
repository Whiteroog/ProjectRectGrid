using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class GroundTilesManager : BaseSquareTileManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public List<GroundTile> GetNeighboursFor(Vector3Int coordinate)
        {
            List<GroundTile> neighboursTileCoordinates = new List<GroundTile>();

            foreach (Vector3Int direction in _directionsSquareTile)
            {
                GroundTile groundTile = GetTile<GroundTile>(coordinate + direction);
                
                if(groundTile is null)
                    continue;
                
                neighboursTileCoordinates.Add(groundTile);
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