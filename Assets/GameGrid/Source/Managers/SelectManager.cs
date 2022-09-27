using GameGrid.Source.Tiles;
using System.Collections.Generic;
using GameGrid.Source.Utils;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        public static SelectManager Instance;

        private bool _isProcessing = false;
        
        private List<GroundTile> _tilesShowingPossibleWays = new();

        private GroundTile _selectedTile;
        private UnitTile _selectedUnit;

        private void Awake()
        {
            Instance = this;
        }

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

            clickPosition.x += 0.5f;
            clickPosition.y += 0.5f;
            clickPosition.z = GroundTilesManager.Instance.transform.position.z;
            Vector3Int clickCoord = GroundTilesManager.Instance.Tilemap.WorldToCell(clickPosition);
            GroundTile selectTile = GroundTilesManager.Instance.FindTile(clickCoord);

            if (selectTile is null)
                return;

            if(_selectedTile is not null)
            {
                if(_selectedTile.Coordinate != selectTile.Coordinate)
                    _selectedTile.TileState.SelectType = TypeSelect.Default;
            }

            switch(selectTile.TileState.SelectType)
            {
                case TypeSelect.Default:
                    {
                        selectTile.TileState.SelectType = TypeSelect.Select;

                        if(selectTile.OccupiedUnit is not null)
                        {
                            _selectedUnit = selectTile.OccupiedUnit;
                            UnitsManager.Instance.GeneratePossibleWays(_selectedUnit.Coordinate, _selectedUnit.GetMovementPoints());
                        }
                        else
                        {
                            _selectedUnit = null;
                            HidePossibleWays();
                        }

                        break;
                    }
                case TypeSelect.Select:
                    {
                        selectTile.TileState.SelectType = TypeSelect.Default;
                        break;
                    }
                case TypeSelect.PossibleWay:
                    {
                        if (_selectedUnit is not null)
                        {
                            UnitsManager.Instance.MoveUnit(_selectedUnit, selectTile.Coordinate, (state) => _isProcessing = state);

                            _selectedUnit = null;
                            HidePossibleWays();
                        }
                        break;
                    }
            }

            _selectedTile = selectTile;
        }
    }
}