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

        private Dictionary<GroundTile, GroundTile> _cachedPastWays;

        [SerializeField] private GroundTilesManager groundTilesManager;

        public void MoveUnit(UnitTile unit ,Vector3Int newCoordinate)
        {
            OnProcessing?.Invoke(this, true);
            StartCoroutine(unit.MovementUnit(newCoordinate, EndProcessing));
        }

        private void EndProcessing()
        {
            OnProcessing?.Invoke(this, false);
        }

        public void GeneratePossibleWays(SelectManager selectManager, UnitTile unit)
        {
            GroundTile groundTileUnderUnit = groundTilesManager.GetTile(unit.Coordinate) as GroundTile;
            _cachedPastWays = BreadthFirstSearch(groundTilesManager, groundTileUnderUnit, unit.GetMovementPoints());

            foreach(GroundTile possibleTile in _cachedPastWays.Keys.Skip(1).ToList())
            {
                selectManager.CreatePointPossibleTiles(possibleTile.Coordinate);
            }
        }

        private Dictionary<GroundTile, GroundTile> BreadthFirstSearch(GroundTilesManager groundTilesManager, GroundTile startNode, int movementPoints)
        {
            Dictionary<GroundTile, GroundTile> visitedNodes = new Dictionary<GroundTile, GroundTile>();
            Dictionary<GroundTile, int> costSoFar = new Dictionary<GroundTile, int>();
            Queue<GroundTile> nodesToVisitQueue = new Queue<GroundTile>();
            
            nodesToVisitQueue.Enqueue(startNode);
            costSoFar[startNode] = 0;
            visitedNodes[startNode] = null;

            while (nodesToVisitQueue.Count > 0)
            {
                GroundTile currentNode = nodesToVisitQueue.Dequeue();
                foreach (GroundTile neighbourNode in groundTilesManager.GetNeighboursFor(currentNode.Coordinate))
                {
                    if(neighbourNode.IsObstacle())
                        continue;

                    int nodeCost = neighbourNode.GetCost();
                    int currentCost = costSoFar[currentNode];
                    int newCost = currentCost + nodeCost;

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