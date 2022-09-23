using System.Collections.Generic;
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

            foreach (Vector3Int direction in directionsSquareTile)
            {
                if(cachedHavingTiles.TryGetValue(coordinate + direction, out BaseSquareTile tile))
                    neighboursTileCoordinates.Add(tile as GroundTile);
            }

            return neighboursTileCoordinates;
        }

        private readonly List<Vector3Int> directionsSquareTile = new List<Vector3Int>()
        {
                            new Vector3Int(0, 1, 0),
            new Vector3Int(-1, 0, 0),     new Vector3Int(1, 0, 0),
                            new Vector3Int(0, -1, 0)
        };
    }
}