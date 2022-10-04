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

        public void ShowPossibleWays(GroundTile showingTile) => _tilesShowingPossibleWays.Add(showingTile);

        public void HidePossibleWays()
        {
            if (_tilesShowingPossibleWays.Count == 0)
                return;

            foreach (GroundTile showingTile in _tilesShowingPossibleWays)
            {
                showingTile.TileState.SelectType = TypeSelect.Default;
                showingTile.CostMovementUnit = 0;
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
                        // select clicked tile
                        selectTile.TileState.SelectType = TypeSelect.Select;
                        _selectTile = selectTile;

                        // state if clicked on unit
                        if (selectTile.OccupiedUnit is not null)
                        {
                            // save unit for using
                            _unitTile = selectTile.OccupiedUnit;
                            
                            // and just highlight possible tile
                            UnitsManager.Instance.GeneratePossibleWays(_unitTile.Coordinate, _unitTile.GetMovementPoints());
                            
                            // if clicked on tile with unit then we have state movement unit
                            _stateSelect = SelectState.ChoicePossiblePathway;
                        }
                        else
                        {
                            // state changing
                            _stateSelect = SelectState.Select;
                        }

                        break;
                    }
                case SelectState.Select:
                    {
                        // when switching tile, past tile turn off, (1)
                        _selectTile.TileState.SelectType = TypeSelect.Default;

                        //************** single case ******************
                        
                        // single case when clicking on same tile
                        if (_selectTile.Coordinate == selectTile.Coordinate)
                        {
                            // cache clear
                            _selectTile = null;

                            // state reset
                            _stateSelect = SelectState.NotSelect;
                            break;
                        }
                        
                        //************** single case ******************

                        // (1) selecting tile turn on
                        selectTile.TileState.SelectType = TypeSelect.Select;
                        
                        // and caching
                        _selectTile = selectTile;

                        // state if clicked on unit
                        if (selectTile.OccupiedUnit is not null)
                        {
                            // save unit for using
                            _unitTile = selectTile.OccupiedUnit;
                            
                            // and just highlight possible tile
                            UnitsManager.Instance.GeneratePossibleWays(_unitTile.Coordinate, _unitTile.GetMovementPoints());
                            
                            // if clicked on tile with unit then we have state movement unit
                            _stateSelect = SelectState.ChoicePossiblePathway;
                        }
                        
                        // state no changing

                        /*
                        else
                        {
                            _stateSelect = SelectState.Select;
                        }
                        */

                        break;
                    }
                case SelectState.ChoicePossiblePathway:
                    {
                        // turn off past tile highlight
                        _selectTile.TileState.SelectType = TypeSelect.Default;

                        //************** single case ******************
                        
                        // single case when clicking on same tile
                        if (_unitTile.Coordinate == selectTile.Coordinate)
                        {
                            // turn off all highlight
                            HidePossibleWays();
                            
                            // clear all cached data
                            _selectTile = null;
                            _unitTile = null;

                            // reset state
                            _stateSelect = SelectState.NotSelect;
                            break;
                        }
                        
                        //************** single case ******************

                        // cached next selected tile
                        _selectTile = selectTile;

                        // if selected not highlight tile
                        if (selectTile.TileState.SelectType != TypeSelect.PossibleWay)
                        {
                            // turn off all highlight
                            HidePossibleWays();
                            
                            // clear unit cache
                            _unitTile = null;
                            
                            // but highlighting select tile
                            selectTile.TileState.SelectType = TypeSelect.Select;

                            // change state
                            _stateSelect = SelectState.Select;
                            break;
                        }
                        
                        // if clicked on highlight tile (possible way)

                        UnitsManager.Instance.MoveUnit(_unitTile, selectTile.Coordinate, (state) => _isProcessing = state);

                        HidePossibleWays();
                        _unitTile = null;

                        // reset state
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