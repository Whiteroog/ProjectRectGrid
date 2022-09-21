using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class SelectTile : BaseSquareTile
    {
        public UnitTile selectedUnitTile;

        public bool HasObject() => selectedUnitTile is not null;

        public void ClearSelectedObject()
        {
            selectedUnitTile = null;
        }

        public void MoveUnit(Vector3Int newCoordinate)
        {
            if(HasObject())
            {
                selectedUnitTile.Coordinate = newCoordinate;
                ClearSelectedObject();
            }
        }
    }
}