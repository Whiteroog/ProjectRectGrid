using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameGrid.Source.Managers
{
    public class GridManager : MonoBehaviour
    {
        private List<BaseSquareTileManager> TileManagers = new();
        private void Awake()
        {
            TileManagers.AddRange(GetComponentsInChildren<BaseSquareTileManager>());
        }

        public List<BaseSquareTileManager> GetTileManagers() => TileManagers;
        public TManager GetTileManager<TManager>() where TManager : class => TileManagers.Find(manager => manager is TManager) as TManager;
    }
}