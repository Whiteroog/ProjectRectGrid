using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class SelectTile : BaseSquareTile
    {
        private UnitTile _selectingUnit;
        public UnitTile SelectingUnit
        {
            set => _selectingUnit = value;
            get
            {
                var _ = _selectingUnit;
                _selectingUnit = null;
                return _;
            }
        }
        public void ClearSelecting() => _selectingUnit = null;
    }
}