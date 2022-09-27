using System.Collections.Generic;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class GroundTilesManager : MonoBehaviour
    {
        public static GroundTilesManager Instance;

        private Dictionary<Vector3Int, GroundTile> _groundTiles = new();

        public Tilemap Tilemap { private set; get; }

        private void Awake()
        {
            Instance = this;

            Tilemap = GetComponent<Tilemap>();

            foreach (GroundTile ground in GetComponentsInChildren<GroundTile>())
            {
                ground.Coordinate = Tilemap.LocalToCell(ground.transform.localPosition);
                _groundTiles[ground.Coordinate] = ground;
            }
        }

        public GroundTile FindTile(Vector3Int coord)
        {
            _groundTiles.TryGetValue(coord, out GroundTile ground);
            return ground;
        }

        public GroundTile[] GetNeighboursFor(Vector3Int centerCoord)
        {
            GroundTile[] neighbours = new GroundTile[4];
            int i = 0;

            foreach (Vector3Int dir in _rectDirections)
            {
                _groundTiles.TryGetValue(centerCoord + dir, out GroundTile ground);
                
                if(ground is null)
                    continue;
                
                neighbours[i++] = ground;
            }

            return neighbours;
        }

        private Vector3Int[] _rectDirections = 
        {
                            new Vector3Int(0, 1, 0),
            new Vector3Int(-1, 0, 0),     new Vector3Int(1, 0, 0),
                            new Vector3Int(0, -1, 0)
        };
    }
}