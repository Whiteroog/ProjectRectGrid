using GameGrid.Source.Tiles;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameGrid.Source.Managers
{
    public class SelectManager : MonoBehaviour
    {
        // private bool _isProcessing = false;
        // private bool _isSelect = false;
        // public bool IsSelect
        // {
        //     private set
        //     {
        //         _isSelect = value;
        //         _selectTile.gameObject.SetActive(value);
        //     }
        //     get => _isSelect;
        // }
        //
        // protected override void Awake()
        // {
        //     base.Awake();
        //     
        //     _selectTile = Instantiate(selectPrefab, transform.position, Quaternion.identity, transform).GetComponent<SelectTile>();
        //     CachingTile(_selectTile);
        //
        //     _selectTile.gameObject.SetActive(false);
        // }
        //
        // public void CreatePointPossibleTiles(Vector3Int spawnCoordinate)
        // {
        //     SelectTile pointTile = Instantiate(pointPrefab, spawnCoordinate, Quaternion.identity, transform).GetComponent<SelectTile>();
        //     CachingTile(pointTile);
        //     _pointPossibleTiles.Add(pointTile);
        // }
        //
        // public void ResetPointPossibleTiles()
        // {
        //     for(int i = _pointPossibleTiles.Count - 1; i >= 0; i--)
        //     {
        //         Destroy(_pointPossibleTiles[i].gameObject);
        //         _pointPossibleTiles.RemoveAt(i);
        //     }
        // }
        //
        // // Event from CameraController
        // public void SelectTile(BaseSquareTile selectedTile)
        // {
        //     if (_isProcessing)
        //         return;
        //
        //     switch (selectedTile.GetTileType())
        //     {
        //         case TileType.Ground:
        //         {
        //             IsSelect = true;
        //             SetTileCoordinate(_selectTile, selectedTile.Coordinate);
        //
        //             // auto clear getter
        //             _selectTile.ClearSelecting();
        //             ResetPointPossibleTiles();
        //             
        //             break;
        //         }
        //         case TileType.UnitPlayer:
        //         {
        //             IsSelect = true;
        //             SetTileCoordinate(_selectTile, selectedTile.Coordinate);
        //
        //             if(selectedTile is UnitTile unitTile)
        //             {
        //                 _selectTile.SelectingUnit = unitTile;
        //
        //                 UnitsManager unitsManager = unitTile.GetTileManager<UnitsManager>();
        //                 unitsManager.GeneratePossibleWays(this, unitTile);
        //             }
        //
        //             break;
        //         }
        //         case TileType.PointWay:
        //         {
        //             IsSelect = false;
        //             SetTileCoordinate(_selectTile, selectedTile.Coordinate);
        //
        //             UnitTile unitTile = _selectTile.SelectingUnit;
        //             
        //             UnitsManager unitsManager = unitTile.GetTileManager<UnitsManager>();
        //             if (unitsManager is not null)
        //             {
        //                 unitsManager.OnProcessing += SetProcessing;
        //                 unitsManager.MoveUnit(unitTile, selectedTile.Coordinate);
        //             }
        //             
        //             ResetPointPossibleTiles();
        //             break;
        //         }
        //         case TileType.Select:
        //         default:
        //         {
        //             IsSelect = false;
        //             
        //             _selectTile.ClearSelecting();
        //             ResetPointPossibleTiles();
        //
        //             break;
        //         }
        //     }
        // }
        //
        // private void SetProcessing(object sender, bool state)
        // {
        //     _isProcessing = state;
        //
        //     if (!state)
        //     {
        //         ((UnitsManager)sender).OnProcessing -= SetProcessing;
        //     }
        // }
    }
}