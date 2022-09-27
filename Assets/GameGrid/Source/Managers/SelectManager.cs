using GameGrid.Source.Tiles;
using System.Collections.Generic;
using GameGrid.Source.Utils;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        private bool _isProcessing = false;
        
        private List<GroundTile> _tilesShowingPossibleWays = new();

        private TypeState _stateType = TypeState.None;

        private GroundTile _selectedTile;

        public void ShowPossibleWays(Vector3Int[] coordsForShow)
        {
            GroundTilesManager groundTilesManager = GroundTilesManager.Instance;
            foreach (Vector3Int coord in coordsForShow)
            {
                GroundTile showingTile = groundTilesManager.FindTile(coord);
                showingTile.TileState.SelectType = TypeSelect.PossibleWay;
                _tilesShowingPossibleWays.Add(showingTile);
            }
        }

        public void HidePossibleWays()
        {
            if (_tilesShowingPossibleWays.Count == 0)
                return;

            foreach (GroundTile showingTile in _tilesShowingPossibleWays)
            {
                showingTile.TileState.SelectType = TypeSelect.Default;
            }
            
            _tilesShowingPossibleWays.Clear();
        }

        // Event from CameraController
        public void SelectTile(Vector3 clickPosition)
        {
            if (_isProcessing)
                return;

            clickPosition.z = GroundTilesManager.Instance.transform.position.z;
            Vector3Int clickCoord = GroundTilesManager.Instance.Tilemap.WorldToCell(clickPosition);
            GroundTile selectTile = GroundTilesManager.Instance.FindTile(clickCoord);

            if (selectTile is null)
                return;

            if (_selectedTile is not null)
                _selectedTile.TileState.SelectType = TypeSelect.Default;

            selectTile.TileState.SelectType = TypeSelect.Select;

            _selectedTile = selectTile;
        }

        private void SetProcessing(object sender, bool state)
        {
            _isProcessing = state;

            if (!state)
            {
                ((UnitsManager)sender).OnProcessing -= SetProcessing;
            }
        }

        enum TypeState
        {
            None,
            SelectGround,
            SelectUnit
        }
    }
}