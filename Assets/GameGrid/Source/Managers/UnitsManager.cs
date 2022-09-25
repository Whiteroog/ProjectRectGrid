using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Tiles;
using GameGrid.Source.Utils;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : BaseSquareTileManager
    {
        public event Action<UnitsManager, bool> OnProcessing;

        private Dictionary<GroundTile, GroundTile> _cachedPathways = new();

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

        public void MoveUnit(UnitTile unitTile ,GroundTile targetTile)
        {
            GroundTile[] pathway = GeneratePathway(targetTile);
            StartCoroutine(MovementUnit(unitTile, pathway));
        }

        private IEnumerator MovementUnit(UnitTile unit, GroundTile[] pathway)
        {
            OnProcessing?.Invoke(this, true);
            
            for (int i = 1; i < pathway.Length; i++)
            {
                Vector3Int start = pathway[i - 1].Coordinate;
                Vector3Int end = pathway[i].Coordinate;

                yield return StartCoroutine(unit.AnimateMovement(start, end));
            }
            
            // temporarily
            SetTileCoordinate(unit, pathway[^1].Coordinate);
            
            OnProcessing?.Invoke(this, false);
        }

        private GroundTile[] GeneratePathway(GroundTile targetTile)
        {
            Stack<GroundTile> pathway = new Stack<GroundTile>();
            
            pathway.Push(targetTile);
            GroundTile nextTile = targetTile;

            while (nextTile != _cachedPathways[nextTile])
            {
                pathway.Push(_cachedPathways[nextTile]);
                nextTile = pathway.Peek();
            }

            return pathway.ToArray();
        }

        public void GeneratePossibleWays(GroundTile sourceTile, UnitTile unit)
        {
            _cachedPathways = BreadthFirstSearch(sourceTile, unit.GetMovementPoints());
            
            GroundTile[] possibleWays = _cachedPathways.Keys.ToArray();

            for (int i = 1; i < possibleWays.Length; i++)
            {
                possibleWays[i].TileState.SetBorderColor(SelectType.PossibleWays);
            }
        }

        private Dictionary<GroundTile, GroundTile> BreadthFirstSearch(GroundTile startTile, int movementPoints)
        {
            Dictionary<GroundTile, GroundTile> visitedTiles = new Dictionary<GroundTile, GroundTile>();
            Dictionary<GroundTile, int> costSoFar = new Dictionary<GroundTile, int>();
            Queue<GroundTile> tilesToVisit = new Queue<GroundTile>();
            
            tilesToVisit.Enqueue(startTile);
            costSoFar[startTile] = 0;
            visitedTiles[startTile] = startTile;

            while (tilesToVisit.Count > 0)
            {
                GroundTile currentTile = tilesToVisit.Dequeue();
                foreach (GroundTile neighbourTile in _groundTilesManager.GetNeighboursFor(currentTile))
                {
                    if(neighbourTile.IsObstacle())
                        continue;

                    int nodeCost = neighbourTile.GetCost();
                    int currentCost = costSoFar[currentTile];
                    int newCost = currentCost + nodeCost;

                    if (newCost <= movementPoints)
                    {
                        if (!visitedTiles.ContainsKey(neighbourTile))
                        {
                            visitedTiles[neighbourTile] = currentTile;
                            costSoFar[neighbourTile] = newCost;
                            tilesToVisit.Enqueue(neighbourTile);
                        }
                        else if (costSoFar[neighbourTile] > newCost)
                        {
                            costSoFar[neighbourTile] = newCost;
                            visitedTiles[neighbourTile] = currentTile;
                        }
                    }
                }
            }

            return visitedTiles;
        }
    }
}