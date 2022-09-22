using UnityEngine;
using UnityEngine.UI;

namespace GameGrid.Source.Tiles
{
    public class SelectTile : BaseSquareTile
    {
        public BaseSquareTile savingTile;

        public bool HasObject() => savingTile is not null;

        public void ClearSelectedObject()
        {
            savingTile = null;
        }
    }
}