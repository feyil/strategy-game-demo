using System;
using System.Collections.Generic;
using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using UnityEngine;

namespace _game.Scripts.ProductionObjects
{
    [Serializable]
    public abstract class ProductionObject : IGridObject
    {
        protected readonly GridManager _gridManager;
        protected readonly ProductionData _productionData;
        protected readonly GridCell[] _regionCells;

        protected float _currentHealth;


        protected ProductionObject(GridManager gridManager, ProductionData productionData, GridCell[] regionCells)
        {
            _gridManager = gridManager;
            _productionData = productionData;
            _regionCells = regionCells;

            _currentHealth = _productionData.Health;
        }

        public void Hit(float damage)
        {
            if (damage >= _currentHealth)
            {
                Destroy();
                _currentHealth = 0;
            }
            else
            {
                _currentHealth -= damage;
            }
        }

        public void Move(GridCell gridCell)
        {
        }

        public virtual void Destroy()
        {
            foreach (var regionCell in _regionCells)
            {
                regionCell.Fill(null);
            }
        }

        public ProductionData GetProductionData()
        {
            return _productionData;
        }

        public List<GridCell> GetAvailableSpawnCells()
        {
            var spawnPoints = new List<GridCell>();
            foreach (var regionCell in _regionCells)
            {
                var cord = regionCell.GetCord();

                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        var aroundCord = new Vector2(cord.x + i, cord.y + j);
                        var cell = _gridManager.GetCell(aroundCord);
                        if (cell != null && !cell.IsFilled())
                        {
                            spawnPoints.Add(cell);
                        }
                    }
                }
            }

            return spawnPoints;
        }
    }
}