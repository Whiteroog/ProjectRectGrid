using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : BaseSquareTileManager
    {
        [SerializeField] private GameObject _tilePrefab;
        private SelectTile _selectTile;

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
            IsSelect = !(selectedTile is SelectTile);

            if(IsSelect)
            {
                if (_selectTile.HasObject())
                {
                    IsSelect = false;
                    _selectTile.MoveUnit(selectedTile.Coordinate);
                }
                else
                {
                    _selectTile.Coordinate = selectedTile.Coordinate;
                    if (selectedTile is UnitTile unitTile)
                    {
                        _selectTile.selectedUnitTile = unitTile;
                    }
                }
            }
            else
            {
                if (_selectTile.HasObject())
                    _selectTile.ClearSelectedObject();
            }
        }
    }
}