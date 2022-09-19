using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Text coordText;
        [SerializeField] private Image selectBorder;

        public Vector3Int Coordinate { private set; get; }

        public void SetCoordinate(Vector3Int newCoordinate)
        {
            Coordinate = newCoordinate;

            coordText.text = $"{Coordinate.x}     {Coordinate.y}";
        }
    }
}
