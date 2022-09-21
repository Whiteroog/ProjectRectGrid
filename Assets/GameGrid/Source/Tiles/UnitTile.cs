using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class UnitTile : BaseSquareTile
    {
        public event Action<UnitTile, bool> OnAnimating;

        public void MoveUnit(Vector3Int newCoordinate)
        {
            Coordinate = newCoordinate;
            OnAnimating?.Invoke(this, true);
        }
    }
}