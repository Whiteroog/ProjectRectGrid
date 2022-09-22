using GameGrid.Source.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class GroundTile : BaseSquareTile
    {
        [SerializeField] private Text coordText;
        [SerializeField] private bool isObstacle = false;
        [SerializeField] private int cost = 1;

        public override Vector3Int Coordinate
        {
            set
            {
                base.Coordinate = value;
                coordText.text = $"{value.x}     {value.y}";
            }
            get => base.Coordinate;
        }

        public bool IsObstacle() => isObstacle;
        public int GetCost() => cost;
    }
}