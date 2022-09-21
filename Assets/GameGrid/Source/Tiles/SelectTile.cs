using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class SelectTile : BaseSquareTile
    {
        public BaseSquareTile selectedTile;

        public bool HasObject() => selectedTile is not null;

        public void ClearSelectedObject()
        {
            selectedTile = null;
        }

        public void MoveUnit(Vector3Int newCoordinate)
        {
            if(HasObject())
            {
                selectedTile.Coordinate = newCoordinate;
                ClearSelectedObject();
            }
        }
    }
}