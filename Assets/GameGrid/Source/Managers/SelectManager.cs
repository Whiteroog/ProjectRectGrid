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

        private SelectState _stateSelect = SelectState.NotSelect;

        private GroundTile _selectTile;
        private GroundTile SelectTile
        {
            set
            {
                if(value is null)
                {
                    _selectTile.TileState.SelectType = TypeSelect.Default;
                    _selectTile = null;
                    _stateSelect = SelectState.NotSelect;
                    return;
                }

                if(_selectTile is null)
                {
                    value.TileState.SelectType = TypeSelect.Select;

                    _selectTile = value;
                    _stateSelect = SelectState.Select;
                    return;
                }

                // единичный случай
                if(_selectTile.Coordinate == value.Coordinate)
                {
                    _selectTile.TileState.SelectType = TypeSelect.Default;
                    _selectTile = null;
                    _stateSelect = SelectState.NotSelect;
                    return;
                }

                _selectTile.TileState.SelectType = TypeSelect.Default;
                value.TileState.SelectType = TypeSelect.Select;

                _selectTile = value;
                _stateSelect = SelectState.Select;
            }
            get => _selectTile;
        }

        private UnitTile _unitTile;
        private UnitTile UnitTile
        {
            set
            {
                if(value is null)
                {
                    _unitTile = null;

                    if(SelectTile is not null)
                        SelectTile = null;

                    HidePossibleWays();
                    _stateSelect = SelectState.NotSelect;

                    return;
                }

                UnitsManager.Instance.GeneratePossibleWays(value.Coordinate, value.GetMovementPoints());
                _unitTile = value;
                _stateSelect = SelectState.ChoicePossiblePathway;
            }
            get => _unitTile;
        }

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
        public void OnSelect(Vector3 clickPosition)
        {
            if (_isProcessing)
                return;

            // offset tile
            clickPosition.x += 0.5f;
            clickPosition.y += 0.5f;
            
            clickPosition.z = GroundTilesManager.Instance.transform.position.z;
            Vector3Int clickCoord = GroundTilesManager.Instance.Tilemap.WorldToCell(clickPosition);
            GroundTile selectTile = GroundTilesManager.Instance.FindTile(clickCoord);

            if (selectTile is null)
                return;

            switch(_stateSelect)
            {
                case SelectState.NotSelect:
                    {
                        SelectTile = selectTile;
                        if(selectTile.OccupiedUnit is not null)
                            UnitTile = selectTile.OccupiedUnit;

                        break;
                    }
                case SelectState.Select:
                    {
                        SelectTile = selectTile;
                        break;
                    }
                case SelectState.ChoicePossiblePathway:
                    {
                        if (UnitTile is not null)
                        {
                            UnitsManager.Instance.MoveUnit(UnitTile, selectTile.Coordinate, (state) => _isProcessing = state);
                            UnitTile = null;
                        }
                        break;
                    }
            }
        }

        enum SelectState
        {
            NotSelect,
            Select,
            ChoicePossiblePathway
        }
    }
}