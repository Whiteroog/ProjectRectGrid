using GameGrid.Source.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class GroundTile : BaseRectTile
    {
        [SerializeField] private Text coordText;
        [SerializeField] private Text costText;
        
        [SerializeField] private int cost = 1;
        [SerializeField] private bool isObstacle = false;

        public UnitTile OccupiedUnit { set; get; }
        
        public TileState TileState { private set; get; }

        public override Vector3Int Coordinate
        {
            set
            {
                base.Coordinate = value;
                coordText.text = $"{value.x}:{value.y}";
            }
            get => base.Coordinate;
        }

        private void Awake()
        {
            TileState = GetComponentInChildren<TileState>();
            
            costText.text = cost.ToString();
        }

        public bool IsObstacle() => isObstacle;
        public int GetCost() => cost;
        public bool IsHaveUnit() => OccupiedUnit is not null;
    }
}