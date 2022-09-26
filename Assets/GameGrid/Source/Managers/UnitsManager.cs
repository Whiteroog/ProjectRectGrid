using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Tiles;
using GameGrid.Source.Utils;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : MonoBehaviour
    {
        public event Action<UnitsManager, bool> OnProcessing;

        private Dictionary<Vector3Int, Vector3Int> _pathways;

        private GroundTilesManager _groundTilesManager;

        private void Awake()
        {
            _groundTilesManager = GetComponentInParent<GroundTilesManager>();
            _pathways = new Dictionary<Vector3Int, Vector3Int>();
        }

        public void MoveUnit(UnitTile unit ,Vector3Int targetCoordinate)
        {
            OnProcessing?.Invoke(this, true);
            StartCoroutine(unit.Move(GeneratePathway(targetCoordinate), EndMoveUnit));
        }

        private void EndMoveUnit(UnitTile unit, Vector3Int targetCoordiante)
        {
            _groundTilesManager.GetGroundTile(unit.Coordinate).OccupiedTileByUnit = null;
            unit.Coordinate = targetCoordiante;
            _groundTilesManager.GetGroundTile(targetCoordiante).OccupiedTileByUnit = unit;

            OnProcessing?.Invoke(this, false);
        }

        private Vector3Int[] GeneratePathway(Vector3Int pointCoordinate)
        {
            Stack<Vector3Int> pathway = new Stack<Vector3Int>();

            while (pointCoordinate != _pathways[pointCoordinate])
            {
                pathway.Push(pointCoordinate);
                pointCoordinate = _pathways[pointCoordinate];

            }

            return pathway.ToArray();
        }

        public void GeneratePossibleWays(SelectManager selectManager, Vector3Int sourceCoordinate, int movementPoints)
        {
            _pathways = BreadthFirstSearch(sourceCoordinate, movementPoints);
            
            Vector3Int[] possibleCoordinates = _pathways.Keys.ToArray();

            for (int i = 1; i < possibleCoordinates.Length; i++)
            {
                selectManager.ShowPossibleWays(possibleCoordinates[i]);
            }
        }

        private Dictionary<Vector3Int, Vector3Int> BreadthFirstSearch(Vector3Int startCoordinate, int movementPoints)
        {
            Dictionary<Vector3Int, Vector3Int> visitedCoordinate = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
            Queue<Vector3Int> coordinateToVisit = new Queue<Vector3Int>();
            
            coordinateToVisit.Enqueue(startCoordinate);
            costSoFar[startCoordinate] = 0;
            visitedCoordinate[startCoordinate] = startCoordinate;

            while (coordinateToVisit.Count > 0)
            {
                Vector3Int currentCoordinate = coordinateToVisit.Dequeue();
                foreach (GroundTile neighbourTile in _groundTilesManager.GetNeighboursFor(currentCoordinate))
                {
                    if(neighbourTile.IsObstacle())
                        continue;

                    int nodeCost = neighbourTile.GetCost();
                    int currentCost = costSoFar[currentCoordinate];
                    int newCost = currentCost + nodeCost;

                    Vector3Int neighbourCoordinate = neighbourTile.Coordinate;

                    if (newCost <= movementPoints)
                    {
                        if (!visitedCoordinate.ContainsKey(neighbourCoordinate))
                        {
                            visitedCoordinate[neighbourCoordinate] = currentCoordinate;
                            costSoFar[neighbourCoordinate] = newCost;
                            coordinateToVisit.Enqueue(neighbourCoordinate);
                        }
                        else if (costSoFar[neighbourCoordinate] > newCost)
                        {
                            costSoFar[neighbourCoordinate] = newCost;
                            visitedCoordinate[neighbourCoordinate] = currentCoordinate;
                        }
                    }
                }
            }

            return visitedCoordinate;
        }
    }
}