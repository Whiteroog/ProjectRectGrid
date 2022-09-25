using GameGrid.Source.Tiles;
using System.Collections.Generic;
using GameGrid.Source.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        private GroundTilesManager _groundTilesManager;

        private bool _isProcessing = false;

        private GroundTile _selectTile;

        private UnitTile _selectUnit;

        private void Awake()
        {
            _groundTilesManager = gridManager.GetTileManager<GroundTilesManager>();
        }

        // Event from CameraController
        public void SelectTile(BaseSquareTile selectedTile)
        {
            if (_isProcessing)
                return;

            _selectTile = _groundTilesManager.GetTile<GroundTile>(selectedTile.Coordinate);

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

                            _selectUnit = null;

                            break;
                        }
                    }

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

                            unitsManager.GeneratePossibleWays(_selectTile, _selectUnit);
                            break;
                        }
                        case SelectType.Select:
                        {
                            _selectTile.TileState.SetBorderColor(SelectType.Default);

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