using GameGrid.Source.Tiles;
using System.Collections.Generic;
using GameGrid.Source.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        private GridManager _gridManager;
        private GroundTilesManager _groundTilesManager;

        private bool _isProcessing = false;

        private UnitTile _selectUnit;

        private List<GroundTile> _showPossibleWays = new List<GroundTile>();

        private GroundTile _selectTile;

        private void Awake()
        {
            _gridManager = GetComponentInParent<GridManager>();
            _groundTilesManager = _gridManager.GetTileManager<GroundTilesManager>();
        }

        public void ShowPossibleWays(GroundTile groundTile)
        {
            _showPossibleWays.Add(groundTile);
            groundTile.TileState.SetBorderColor(SelectType.PossibleWays);
        }

        public void ClearPossibleWays()
        {
            foreach (GroundTile groundTile in _showPossibleWays)
            {
                groundTile.TileState.SetBorderColor(SelectType.Default);
            }
            
            _showPossibleWays.Clear();
        }

        // Event from CameraController
        public void SelectTile(BaseRectTile selectedTile)
        {
            if (_isProcessing)
                return;

            GroundTile newSelectTile = _groundTilesManager.GetTile<GroundTile>(selectedTile.Coordinate);
            
            if (_selectTile is not null)
            {
                if(_selectTile.Coordinate != newSelectTile.Coordinate)
                    _selectTile.TileState.SetBorderColor(SelectType.Default);
            }

            _selectTile = newSelectTile;

            switch (selectedTile.GetTileType())
            {
                case TileType.Ground:
                {
                    switch (_selectTile.TileState.CurrentSelectType)
                    {
                        case SelectType.Default:
                        {
                            _selectTile.TileState.SetBorderColor(SelectType.Select);
                            break;
                        }
                        case SelectType.Select:
                        {
                            _selectTile.TileState.SetBorderColor(SelectType.Default);
                            break;
                        }
                        case SelectType.PossibleWays:
                        {
                            UnitsManager unitsManager = _selectUnit.GetTileManager<UnitsManager>();
                            if (unitsManager is not null)
                            {
                                unitsManager.OnProcessing += SetProcessing;
                                unitsManager.MoveUnit(_selectUnit, _selectTile);
                            }
                            break;
                        }
                    }
                    _selectUnit = null;
                    ClearPossibleWays();
                    break;
                }
                case TileType.Unit:
                {
                    switch (_selectTile.TileState.CurrentSelectType)
                    {
                        case SelectType.Default:
                        {
                            _selectTile.TileState.SetBorderColor(SelectType.Select);

                            _selectUnit = _selectTile.GetOccupiedTile<UnitTile>();
                            UnitsManager unitsManager = _selectUnit.GetTileManager<UnitsManager>();

                            unitsManager.GeneratePossibleWays(this, _selectTile, _selectUnit);
                            break;
                        }
                        case SelectType.Select:
                        {
                            _selectTile.TileState.SetBorderColor(SelectType.Default);
                            ClearPossibleWays();
                            
                            _selectUnit = null;
                            break;
                        }
                    }

                    break;
                }
            }
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