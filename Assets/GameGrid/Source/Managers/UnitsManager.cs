using System;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : BaseSquareTileManager
    {
        public event Action<UnitsManager, bool> OnProcessing;

        private Dictionary<Vector3Int, Vector3Int?> _cachedPathways = new();

        public void MoveUnit(UnitTile unit ,Vector3Int newCoordinate)
        {
            OnProcessing?.Invoke(this, true);
            
            StartCoroutine(unit.MovementUnit(GeneratePathway(newCoordinate), EndProcessing));
            // StartCoroutine(unit.MovementUnitNotTile(newCoordinate, EndProcessing));
        }

        private void EndProcessing()
        {
            OnProcessing?.Invoke(this, false);
        }

        public List<Vector3Int> GeneratePathway(Vector3Int targetNode)
        {
            if (_cachedPathways.Count == 0)
                return new List<Vector3Int>();
            
            List<Vector3Int> pathway = new List<Vector3Int>() { targetNode };

            Vector3Int nextNode = targetNode;
            while (_cachedPathways[nextNode] is not null)
            {
                pathway.Add(_cachedPathways[nextNode].Value);
                nextNode = _cachedPathways[nextNode].Value;
            }
            pathway.Reverse();

            return pathway;
        }

        public void GeneratePossibleWays(SelectManager selectManager, UnitTile unit)
        {
            GroundTilesManager groundTilesManager = GridSystem.GetManager<GroundTilesManager>();

            _cachedPathways = BreadthFirstSearch(groundTilesManager, unit.Coordinate, unit.GetMovementPoints());
            
            List<Vector3Int> possibleWays = _cachedPathways.Keys.ToList();
            possibleWays.RemoveAt(0);
            
            foreach(Vector3Int possibleWay in possibleWays)
            {
                selectManager.CreatePointPossibleTiles(possibleWay);
            }
        }

        private Dictionary<Vector3Int, Vector3Int?> BreadthFirstSearch(GroundTilesManager groundTilesManager, Vector3Int startNode, int movementPoints)
        {
            Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
            Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
            Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();
            
            nodesToVisitQueue.Enqueue(startNode);
            costSoFar[startNode] = 0;
            visitedNodes[startNode] = null;

            while (nodesToVisitQueue.Count > 0)
            {
                Vector3Int currentNode = nodesToVisitQueue.Dequeue();
                foreach (GroundTile neighbourTile in groundTilesManager.GetNeighboursFor(currentNode))
                {
                    if(neighbourTile.IsObstacle())
                        continue;

                    int nodeCost = neighbourTile.GetCost();
                    int currentCost = costSoFar[currentNode];
                    int newCost = currentCost + nodeCost;

                    Vector3Int neighbourNode = neighbourTile.Coordinate;

                    if (newCost <= movementPoints)
                    {
                        if (!visitedNodes.ContainsKey(neighbourNode))
                        {
                            visitedNodes[neighbourNode] = currentNode;
                            costSoFar[neighbourNode] = newCost;
                            nodesToVisitQueue.Enqueue(neighbourNode);
                        }
                        else if (costSoFar[neighbourNode] > newCost)
                        {
                            costSoFar[neighbourNode] = newCost;
                            visitedNodes[neighbourNode] = currentNode;
                        }
                    }
                }
            }

            return visitedNodes;
        }
    }
}