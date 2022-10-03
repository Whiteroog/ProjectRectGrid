using GameGrid.Source.Tiles;
using System.Collections.Generic;
using GameGrid.Source.Utils;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        public static SelectManager Instance;

        [SerializeField] private LayerMask selectMask;

        private bool _isProcessing = false;
        
        private List<GroundTile> _tilesShowingPossibleWays = new();

        private SelectState _stateSelect = SelectState.NotSelect;

        private GroundTile _selectTile;
        private UnitTile _unitTile;

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

            GroundTile selectTile = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity, selectMask)
                .collider?.gameObject.GetComponent<GroundTile>();

            if (selectTile is null)
                return;

            switch(_stateSelect)
            {
                case SelectState.NotSelect:
                    {
                        selectTile.TileState.SelectType = TypeSelect.Select;
                        _selectTile = selectTile;

                        if (selectTile.OccupiedUnit is not null)
                        {
                            _unitTile = selectTile.OccupiedUnit;
                            UnitsManager.Instance.GeneratePossibleWays(_unitTile.Coordinate, _unitTile.GetMovementPoints());
                            _stateSelect = SelectState.ChoicePossiblePathway;
                        }
                        else
                        {
                            _stateSelect = SelectState.Select;
                        }

                        break;
                    }
                case SelectState.Select:
                    {
                        _selectTile.TileState.SelectType = TypeSelect.Default;

                        if (_selectTile.Coordinate == selectTile.Coordinate)
                        {
                            _selectTile = null;

                            _stateSelect = SelectState.NotSelect;
                            break;
                        }

                        selectTile.TileState.SelectType = TypeSelect.Select;
                        _selectTile = selectTile;

                        if (selectTile.OccupiedUnit is not null)
                        {
                            _unitTile = selectTile.OccupiedUnit;
                            UnitsManager.Instance.GeneratePossibleWays(_unitTile.Coordinate, _unitTile.GetMovementPoints());
                            _stateSelect = SelectState.ChoicePossiblePathway;
                        }
                        else
                        {
                            _stateSelect = SelectState.Select;
                        }

                        break;
                    }
                case SelectState.ChoicePossiblePathway:
                    {
                        _selectTile.TileState.SelectType = TypeSelect.Default;

                        if (_unitTile.Coordinate == selectTile.Coordinate)
                        {
                            HidePossibleWays();
                            _selectTile = null;
                            _unitTile = null;

                            _stateSelect = SelectState.NotSelect;
                            break;
                        }

                        _selectTile = selectTile;

                        if (selectTile.TileState.SelectType != TypeSelect.PossibleWay)
                        {
                            HidePossibleWays();
                            _unitTile = null;
                            selectTile.TileState.SelectType = TypeSelect.Select;

                            _stateSelect = SelectState.Select;
                            break;
                        }

                        UnitsManager.Instance.MoveUnit(_unitTile, selectTile.Coordinate, (state) => _isProcessing = state);

                        HidePossibleWays();
                        _unitTile = null;

                        _stateSelect = SelectState.NotSelect;
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