using System;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : MonoBehaviour
    {
        public static UnitsManager Instance;

        private Dictionary<Vector3Int, Vector3Int> _pathways = new();

        private UnitTile[] unitTiles;

        public Tilemap Tilemap { private set; get; }

        private void Awake()
        {
            Instance = this;

            Tilemap = GetComponent<Tilemap>();

            unitTiles = GetComponentsInChildren<UnitTile>();

            GroundTilesManager groundTilesManager = GroundTilesManager.Instance;

            foreach(UnitTile unit in unitTiles)
            {
                unit.Coordinate = Tilemap.LocalToCell(unit.transform.localPosition);
                groundTilesManager.FindTile(unit.Coordinate).OccupiedUnit = unit;
            }
        }

        public void MoveUnit(UnitTile unit ,Vector3Int targetCoord, Action<bool> onProcessing)
        {
            onProcessing.Invoke(true);
            StartCoroutine(unit.Move(GeneratePathway(targetCoord), () => onProcessing.Invoke(false)));
        }

        private Vector3Int[] GeneratePathway(Vector3Int targetCoord)
        {
            Stack<Vector3Int> pathway = new Stack<Vector3Int>();
            pathway.Push(targetCoord);

            //        current way -> past way
            while (pathway.Peek() != _pathways[pathway.Peek()])
            {
                pathway.Push(_pathways[pathway.Peek()]);
            }

            return pathway.ToArray();
        }

        public void GeneratePossibleWays(Vector3Int targetCoord, int movementPoints)
        {
            _pathways = BreadthFirstSearch(targetCoord, movementPoints);
            SelectManager.Instance.ShowPossibleWays(_pathways.Keys.ToArray()[1..]);
        }

        private Dictionary<Vector3Int, Vector3Int> BreadthFirstSearch(Vector3Int startCoord, int movementPoints)
        {
            Dictionary<Vector3Int, Vector3Int> visitedCoords = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
            Queue<Vector3Int> coordsToVisit = new Queue<Vector3Int>();
            
            coordsToVisit.Enqueue(startCoord);
            costSoFar[startCoord] = 0;
            visitedCoords[startCoord] = startCoord;

            while (coordsToVisit.Count > 0)
            {
                Vector3Int currentCoord = coordsToVisit.Dequeue();
                foreach (GroundTile neighbourTile in GroundTilesManager.Instance.GetNeighboursFor(currentCoord))
                {
                    if (neighbourTile is null)
                        continue;

                    if(neighbourTile.IsObstacle())
                        continue;

                    int nodeCost = neighbourTile.GetCost();
                    int currentCost = costSoFar[currentCoord];
                    int newCost = currentCost + nodeCost;

                    Vector3Int neighbourCoord = neighbourTile.Coordinate;

                    if (newCost <= movementPoints)
                    {
                        if (!visitedCoords.ContainsKey(neighbourCoord))
                        {
                            visitedCoords[neighbourCoord] = currentCoord;
                            costSoFar[neighbourCoord] = newCost;
                            coordsToVisit.Enqueue(neighbourCoord);
                        }
                        else if (costSoFar[neighbourCoord] > newCost)
                        {
                            costSoFar[neighbourCoord] = newCost;
                            visitedCoords[neighbourCoord] = currentCoord;
                        }
                    }
                }
            }

            return visitedCoords;
        }
    }
}