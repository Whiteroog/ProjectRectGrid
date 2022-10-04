using GameGrid.Source.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class GroundTile : BaseRectTile
    {
        [SerializeField] private Text coordText;
        [SerializeField] private Text costText;

        [SerializeField] private Text costMovementUnitText;

        
        [SerializeField] private int cost = 1;
        [SerializeField] private bool isObstacle = false;

        private int _costMovementUnit = 0;

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

        public int CostMovementUnit
        {
            set
            {
                _costMovementUnit = value;

                if (value != 0)
                    costMovementUnitText.text = value.ToString();
                else
                    costMovementUnitText.text = "";
            }
            get => _costMovementUnit;
        }

        private void Awake()
        {
            TileState = GetComponentInChildren<TileState>();
            
            costText.text = cost.ToString();
            costMovementUnitText.text = "";
        }

        public bool IsObstacle() => isObstacle;
        public int GetCost() => cost;
    }
}