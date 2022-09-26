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
                showingTile.TileState.SetBorderColor(TypeSelect.Default);
            }
            
            _tilesShowingPossibleWays.Clear();
        }

        // Event from CameraController
        public void DefineTile(Vector3 clickPosition)
        {
            if (_isProcessing)
                return;

            GroundTile selectTile = GroundTilesManager.Instance.FindTile(GroundTilesManager.Instance.Tilemap.WorldToCell(clickPosition));

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