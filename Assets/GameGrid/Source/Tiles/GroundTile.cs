using System;
using GameGrid.Source.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class GroundTile : BaseSquareTile
    {
        [SerializeField] private Text coordinateText;
        [SerializeField] private Text costText;
        
        [SerializeField] private int cost = 1;
        [SerializeField] private bool isObstacle = false;

        private BaseSquareTile _occupiedTile;

        public override Vector3Int Coordinate
        {
            set
            {
                base.Coordinate = value;
                coordinateText.text = $"{value.x}:{value.y}";
            }
            get => base.Coordinate;
        }

        private void Awake()
        {
            costText.text = cost.ToString();
        }

        public bool IsObstacle() => isObstacle;
        
        public int GetCost() => cost;

        public void SetOccupiedTile(BaseSquareTile newTile) => _occupiedTile = newTile;
        public TTile GetOccupiedTile<TTile>() where TTile : class => _occupiedTile as TTile;
    }
}