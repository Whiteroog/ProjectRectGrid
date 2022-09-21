using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : BaseSquareTileManager
    {
        [SerializeField] private GameObject _tilePrefab;
        private SelectTile _selectTile;

        private bool _isProcessing = false;

        private bool _isSelect = false;

        private BaseSquareTile _cachedSelectedTile;
        
        public bool IsSelect
        {
            private set
            {
                _isSelect = value;
                _selectTile.gameObject.SetActive(value);
            }
            get => _isSelect;
        }

        private void Awake()
        {
            Vector3Int spawnPosition = tilemap.LocalToCell(transform.position);
            _selectTile = Instantiate(_tilePrefab, spawnPosition, Quaternion.identity, transform).GetComponent<SelectTile>();
            _selectTile.Coordinate = spawnPosition;
            _selectTile.gameObject.SetActive(false);
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
                        if(_selectTile.selectedTile is UnitTile unitTile)
                        {
                            unitTile.OnAnimating += SetProcessing;
                            unitTile.MoveUnit(selectedTile.Coordinate);
                            _selectTile.ClearSelectedObject();
                        }
                    }
                    break;
                }
                case TileType.Unit:
                {
                    print("Select unit");
                    IsSelect = true;
                    _selectTile.Coordinate = selectedTile.Coordinate;
                    
                    _selectTile.selectedTile = selectedTile;
                    break;
                }
                case TileType.Select:
                case TileType.None:
                default:
                {
                    IsSelect = false;
                    if (_selectTile.HasObject())
                        _selectTile.ClearSelectedObject();
                    
                    break;
                }
            }
        }

        public void SetProcessing(object sender, bool state)
        {
            _isProcessing = state;

            if (!state)
            {
                ((UnitTile)sender).OnAnimating -= SetProcessing;
            }
        }
    }
}