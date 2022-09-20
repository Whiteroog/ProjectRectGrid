using GameGrid.Source.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class GroundTile : BaseSquareTile
    {
        [SerializeField] private Text coordText;

        public override Vector3Int Coordinate
        {
            protected set
            {
                base.Coordinate = value;
                coordText.text = $"{value.x}     {value.y}";
            }
            get => base.Coordinate;
        }
    }
}