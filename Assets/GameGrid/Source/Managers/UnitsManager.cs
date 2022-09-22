using System;
using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class UnitsManager : BaseSquareTileManager
    {
        public event Action<UnitsManager, bool> OnProcessing;
        public void MoveUnit(UnitTile unit ,Vector3Int newCoordinate)
        {
            OnProcessing?.Invoke(this, true);
            StartCoroutine(unit.MovementUnit(newCoordinate, EndProcessing));
        }

        public void EndProcessing()
        {
            OnProcessing?.Invoke(this, false);
        }
    }
}