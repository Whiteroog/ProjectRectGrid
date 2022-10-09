using System;
using System.Collections.Generic;
using System.Linq;
using GameGrid.Source.Systems;
using GameGrid.Source.Tiles;
using GameGrid.Source.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : MonoBehaviour
    {
        public static UnitsManager Instance;

        private Dictionary<GroundTile, GroundTile> _pathways = new();
        private Dictionary<GroundTile, int> _costPathways = new();

        private UnitTile[] _unitTiles;

        private void Awake()
        {
            Instance = this;

            _unitTiles = GetComponentsInChildren<UnitTile>();

            GroundTilesManager groundTilesManager = GroundTilesManager.Instance;

            foreach(UnitTile unit in _unitTiles)
            {
                unit.Coordinate = GridSystem.Instance.ConvertToGridCoordinate(unit.transform.localPosition);
                GroundTile foundTile = groundTilesManager.FindTile(unit.Coordinate);
                foundTile.OccupiedUnit = unit;
            }
        }

        public void ResetMovementPointsOfUnits()
        {
            foreach(UnitTile unit in _unitTiles)
            {
                unit.ResetMovementPoints();
            }
        }

        public void MoveUnit(UnitTile unit ,GroundTile targetTile)
        {
            GroundTile[] pathway = GeneratePathway(targetTile);

            if (pathway.Length == 1)
                return;

            SelectManager.Instance.IsProcessing = true;
            
            unit.Move(pathway, pathway[^1].CostMovementUnit);
        }

        private GroundTile[] GeneratePathway(GroundTile targetTile)
        {
            Stack<GroundTile> pathway = new Stack<GroundTile>();
            pathway.Push(targetTile);

            //        current way -> past way
            while (pathway.Peek() != _pathways[pathway.Peek()])
            {
                pathway.Push(_pathways[pathway.Peek()]);
            }

            return pathway.ToArray();
        }

        public void GeneratePossibleWays(GroundTile targetTile, int movementPoints)
        {
            BreadthFirstSearch(targetTile, movementPoints);

            foreach (GroundTile tile in _pathways.Keys.ToArray()[1..])
            {
                tile.TileState.SelectType = TypeSelect.PossibleWay;
                tile.CostMovementUnit = _costPathways[tile];

                SelectManager.Instance.ShowPossibleWays(tile);
            }
        }

        private void BreadthFirstSearch(GroundTile startTile, int movementPoints)
        {
            Dictionary<GroundTile, GroundTile> visitedTiles = new();
            Dictionary<GroundTile, int> costSoFar = new();
            Queue<GroundTile> tilesToVisit = new();
            
            tilesToVisit.Enqueue(startTile);
            costSoFar[startTile] = 0;
            visitedTiles[startTile] = startTile;

            while (tilesToVisit.Count > 0)
            {
                GroundTile currentTile = tilesToVisit.Dequeue();
                foreach (GroundTile neighbourTile in GroundTilesManager.Instance.GetNeighboursFor(currentTile.Coordinate))
                {
                    if (neighbourTile is null)
                        continue;

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

            _pathways = visitedTiles;
            _costPathways = costSoFar;
        }
    }
}