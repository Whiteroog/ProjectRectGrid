using GameGrid.Source.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : BaseSquareTileManager
    {
        [SerializeField] private GameObject selectPrefab;
        [SerializeField] private GameObject pointPrefab;

        private SelectTile _selectTile;
        private List<SelectTile> _pointPossibleTiles = new List<SelectTile>();

        private bool _isProcessing = false;
        private bool _isSelect = false;

        public bool IsSelect
        {
            private set
            {
                _isSelect = value;
                _selectTile.gameObject.SetActive(value);
            }
            get => _isSelect;
        }

        protected override void Awake()
        {
            Vector3Int spawnPosition = tilemap.LocalToCell(transform.position);
            _selectTile = Instantiate(selectPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<SelectTile>();
            
            base.Awake();

            _selectTile.gameObject.SetActive(false);
        }

        public void CreatePointPossibleTiles(Vector3Int spawnCoordinate)
        {
            SelectTile pointTile = Instantiate(pointPrefab, spawnCoordinate, Quaternion.identity, transform).GetComponent<SelectTile>();
            CachingTile(pointTile);
            _pointPossibleTiles.Add(pointTile);
        }

        public void ResetPointPossibleTiles()
        {
            for(int i = _pointPossibleTiles.Count - 1; 0 <= i; i--)
            {
                Destroy(_pointPossibleTiles[i].gameObject);
                _pointPossibleTiles.RemoveAt(i);
            }
        }

        // Event from CameraController
        public void SelectTile(BaseSquareTile selectedTile)
        {
            if (_isProcessing)
                return;

            switch (selectedTile.GetTileType())
            {
                case TileType.Ground:
                {
                    IsSelect = true;
                    _selectTile.Coordinate = selectedTile.Coordinate;
                    
                    if (_selectTile.HasObject())
                    {
                        IsSelect = false;
                        if(_selectTile.savingTile.GetTileManager() is UnitsManager unitManager)
                        {
                            unitManager.OnProcessing += SetProcessing;
                            unitManager.MoveUnit(_selectTile.savingTile as UnitTile, selectedTile.Coordinate);
                            _selectTile.ClearSelectedObject();

                                if (_pointPossibleTiles.Count > 0)
                                    ResetPointPossibleTiles();
                            }
                    }
                    break;
                }
                case TileType.Unit:
                {
                    print("Select unit");
                    IsSelect = true;

                    if(selectedTile is UnitTile unitTile)
                    {
                            _selectTile.Coordinate = unitTile.Coordinate;
                            _selectTile.savingTile = unitTile;

                            if (unitTile.GetTileManager() is UnitsManager unitManager )
                            {
                                unitManager.GeneratePossibleWays(this, unitTile);
                            }
                    }

                    break;
                }
                case TileType.Select:
                case TileType.None:
                default:
                {
                    IsSelect = false;
                    if (_selectTile.HasObject())
                        _selectTile.ClearSelectedObject();

                        if (_pointPossibleTiles.Count > 0)
                            ResetPointPossibleTiles();
                    

                        break;
                }
            }
        }

        public void SetProcessing(object sender, bool state)
        {
            _isProcessing = state;

            if (!state)
            {
                ((UnitsManager)sender).OnProcessing -= SetProcessing;
            }
        }
    }
}