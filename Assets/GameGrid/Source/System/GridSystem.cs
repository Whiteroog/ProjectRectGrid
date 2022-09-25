using System;
using System.Collections.Generic;
using GameGrid.Source.Managers;
using GameGrid.Source.Tiles;
using UnityEngine;

namespace GameGrid.Source.System
{
    public class GridSystem : MonoBehaviour
    {
        private List<BaseSquareTileManager> TileManagers = new();
        private void Awake()
        {
            TileManagers.AddRange(GetComponentsInChildren<BaseSquareTileManager>());
        }

        public TManager GetManager<TManager>() where TManager : class => TileManagers.Find(manager => manager is TManager) as TManager;
    }
}