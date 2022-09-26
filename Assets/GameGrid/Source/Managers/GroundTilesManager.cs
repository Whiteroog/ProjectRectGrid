using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class GroundTilesManager : MonoBehaviour
    {
        private Dictionary<Vector3Int, GroundTile> _groundTiles = new();

        public Tilemap Tilemap { private set; get; }

        private void Awake()
        {
            Tilemap = GetComponent<Tilemap>();

            foreach(GroundTile groundTile in GetComponentsInChildren<GroundTile>())
            {
                _groundTiles[groundTile.Coordinate] = groundTile;
            }
        }

        public GroundTile GetGroundTile(Vector3Int coordiante)
        {
            _groundTiles.TryGetValue(coordiante, out GroundTile groundTile);
            return groundTile;
        }

        public GroundTile[] GetNeighboursFor(Vector3Int centerCoordiante)
        {
            GroundTile[] neighboursTiles = new GroundTile[4];
            int i = 0;

            foreach (Vector3Int direction in _directionsSquareTile)
            {
                GroundTile groundTile = _groundTiles[centerCoordiante + direction];
                
                if(groundTile is null)
                    continue;
                
                neighboursTiles[i++] = groundTile;
            }

            return neighboursTiles;
        }

        private Vector3Int[] _directionsSquareTile = 
        {
                            new Vector3Int(0, 1, 0),
            new Vector3Int(-1, 0, 0),     new Vector3Int(1, 0, 0),
                            new Vector3Int(0, -1, 0)
        };
    }
}