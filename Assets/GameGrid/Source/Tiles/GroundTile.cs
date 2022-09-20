using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class GroundTile : Tile
    {
        [SerializeField] private Text coordText;

        public override void SetCoordinate(Vector3Int newCoordinate)
        {
            base.SetCoordinate(newCoordinate);
            
            coordText.text = $"{Coordinate.x}     {Coordinate.y}";
        }
    }
}