using GameGrid.Source.Tiles;
using System.Collections.Generic;
using GameGrid.Source.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        private bool _isProcessing = false;

        private UnitTile _selectUnit;
        private GroundTile _pastSelectTile;
        private List<GroundTile> _showPossibleWays = new List<GroundTile>();

        private GroundTilesManager _groundTilesManager;

        private void Awake()
        {
            _groundTilesManager = GetComponentInParent<GroundTilesManager>();
        }

        public void ShowPossibleWays(Vector3Int coordinate)
        {
            GroundTile groundTile = _groundTilesManager.GetGroundTile(coordinate);
            groundTile.TileState.SetBorderColor(TypeSelect.PossibleWays);
            _showPossibleWays.Add(groundTile);
        }

        public void ClearPossibleWays()
        {
            if (_showPossibleWays.Count == 0)
                return;

            foreach (GroundTile groundTile in _showPossibleWays)
            {
                groundTile.TileState.SetBorderColor(TypeSelect.Default);
            }
            
            _showPossibleWays.Clear();
        }

        // Event from CameraController
        public void DefineTile(Vector3 clickPosition)
        {
            if (_isProcessing)
                return;

            GroundTile selectTile = _groundTilesManager.GetGroundTile(_groundTilesManager.Tilemap.WorldToCell(clickPosition));

            if (selectTile is null)
                return;

            // TODO ddoing
        }

        private void SetProcessing(object sender, bool state)
        {
            _isProcessing = state;

            if (!state)
            {
                ((UnitsManager)sender).OnProcessing -= SetProcessing;
            }
        }
    }
}