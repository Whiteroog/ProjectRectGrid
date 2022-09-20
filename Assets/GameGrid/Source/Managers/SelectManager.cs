using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class SelectManager : BaseSquareTileManager
    {
        [SerializeField] private GameObject _tilePrefab;
        private SelectTile _selectTile;
        
        private void Awake()
        {
            _selectTile = Instantiate(_tilePrefab).GetComponent<SelectTile>();
            _selectTile.gameObject.transform.position = tilemap.LocalToCell(_selectTile.gameObject.transform.position);
            // _selectTile.gameObject.SetActive(false);
        }
    }
}