using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : BaseSquareTileManager
    {
        public event Action<UnitsManager, bool> OnProcessing;

        private Dictionary<Vector3Int, Vector3Int> _cachedPathways = new();

        private GroundTilesManager _groundTilesManager;

        protected override void Awake()
        {
            base.Awake();

            _groundTilesManager = GridManager.GetTileManager<GroundTilesManager>();

            foreach (var unit in CachedTiles)
            {
                GroundTile groundTile = _groundTilesManager.GetTile<GroundTile>(unit.Key);
                groundTile.SetOccupiedTile(unit.Value);
            }
        }

        public void MoveUnit(UnitTile unitTile ,Vector3Int targetCoordinate)
        {
            Vector3Int[] pathway = GeneratePathway(targetCoordinate);
            StartCoroutine(MovementUnit(unitTile, pathway));
        }

        private IEnumerator MovementUnit(UnitTile unit, Vector3Int[] pathway)
        {
            OnProcessing?.Invoke(this, true);
            
            for (int i = 1; i < pathway.Length; i++)
            {
                Vector3Int start = pathway[i - 1];
                Vector3Int end = pathway[i];

                yield return StartCoroutine(unit.AnimateMovement(start, end));
            }
            
            SetTileCoordinate(unit, pathway[^1]);
            
            OnProcessing?.Invoke(this, false);
        }

        private Vector3Int[] GeneratePathway(Vector3Int targetNode)
        {
            Stack<Vector3Int> pathway = new Stack<Vector3Int>();
            
            pathway.Push(targetNode);
            Vector3Int nextNode = targetNode;

            while (nextNode != _cachedPathways[nextNode])
            {
                pathway.Push(_cachedPathways[nextNode]);
                nextNode = pathway.Peek();
            }

            return pathway.ToArray();
        }

        public void GeneratePossibleWays(SelectManager selectManager, UnitTile unit)
        {
            _cachedPathways = BreadthFirstSearch(unit.Coordinate, unit.GetMovementPoints());
            
            Vector3Int[] possibleWays = _cachedPathways.Keys.ToArray();

            for (int i = 1; i < possibleWays.Length; i++)
            {
                //selectManager.CreatePointPossibleTiles(possibleWays[i]);
            }
        }

        private Dictionary<Vector3Int, Vector3Int> BreadthFirstSearch(Vector3Int startNode, int movementPoints)
        {
            Dictionary<Vector3Int, Vector3Int> visitedNodes = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
            Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();
            
            nodesToVisitQueue.Enqueue(startNode);
            costSoFar[startNode] = 0;
            visitedNodes[startNode] = startNode;

            while (nodesToVisitQueue.Count > 0)
            {
                Vector3Int currentNode = nodesToVisitQueue.Dequeue();
                foreach (GroundTile neighbourTile in _groundTilesManager.GetNeighboursFor(currentNode))
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