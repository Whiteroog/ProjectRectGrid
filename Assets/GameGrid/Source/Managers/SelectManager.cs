using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : BaseSquareTileManager
    {
        [SerializeField] private GameObject _tilePrefab;
        private SelectTile _selectTile;

        public bool IsSelecting { private set; get; } = false;
        public bool IsSelectingUnit { private set; get; } = false;
        
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
            if (IsSelecting && _selectTile.Coordinate == selectedTile.Coordinate || selectedTile is UnitTile)
            {
                IsSelecting = IsSelectingUnit = false;
                _selectTile.gameObject.SetActive(false);
            }
            else
            {
                IsSelecting = true;
                IsSelectingUnit = selectedTile is UnitTile;
                _selectTile.gameObject.SetActive(true);
                _selectTile.Coordinate = selectedTile.Coordinate;
            }
        }
    }
}