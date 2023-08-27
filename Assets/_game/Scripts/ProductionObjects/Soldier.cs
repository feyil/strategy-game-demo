using System;
using System.Collections;
using System.Collections.Generic;
using _game.Scripts.Core;
using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using _game.Scripts.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _game.Scripts.ProductionObjects
{
    public class Soldier : IGridObject
    {
        private readonly ProductionUnitData _productionUnitData;
        private GridCell _gridCell;
        private bool _isSelected;
        private readonly GridManager _gridManager;
        private Coroutine _traverseAnimation;
        private float _currentHealth;
        private readonly WaitForSeconds _waitForTraverseStep = new(0.2f);

        public Soldier(GridManager gridManager, ProductionUnitData productionUnitData, GridCell gridCell)
        {
            _gridManager = gridManager;
            _productionUnitData = productionUnitData;
            _gridCell = gridCell;
            _currentHealth = productionUnitData.Health;
        }

        public void Destroy()
        {
            if (_traverseAnimation != null)
            {
                GameManager.Instance.StopCoroutine(_traverseAnimation);
            }

            _gridCell.Fill(null);
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

        public float GetHealth()
        {
            return _currentHealth;
        }

        private IEnumerator Attack(IGridObject target)
        {
            if (target == null) yield break;
            while (target.GetHealth() > 0)
            {
                target.Hit(_productionUnitData.Damage);
                yield return _waitForTraverseStep;
            }
        }

        private GridCell CanAttack(GridCell gridCell)
        {
            if (!gridCell.IsFilled()) return gridCell;

            var availableSpawnCells = gridCell.GetGridObject().GetAvailableSpawnCells();
            var count = availableSpawnCells.Count;
            if (count == 0) return gridCell;

            var randomAttackPoint = availableSpawnCells[Random.Range(0, count)];
            return randomAttackPoint;
        }

        public void Move(GridCell gridCell)
        {
            var target = CanAttack(gridCell);
            if (target.IsFilled() || gridCell == _gridCell) return;
            var path = AStarPathfinder.GetPath(_gridManager, _gridCell.GetCord(), target.GetCord());

            _traverseAnimation = GameManager.Instance.StartCoroutine(TraverseAnimation(path, () =>
            {
                if (target == gridCell) return;
                GameManager.Instance.StartCoroutine(Attack(gridCell.GetGridObject()));
            }));
        }

        public List<GridCell> GetAvailableSpawnCells()
        {
            var spawnPoints = new List<GridCell>();

            var cord = _gridCell.GetCord();

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

            return spawnPoints;
        }

        private IEnumerator TraverseAnimation(GridCell[] path, Action onComplete)
        {
            foreach (var gridCell in path)
            {
                yield return _waitForTraverseStep;
                _gridCell.Fill(null);
                _gridCell = gridCell;
                gridCell.FillSpecific(this, _productionUnitData.Color);
            }

            onComplete?.Invoke();
        }

        public bool IsSelected()
        {
            return _isSelected;
        }

        public void Select(bool state)
        {
            _isSelected = state;
            if (_isSelected)
            {
                _gridCell.SetColor(Color.black);
            }
            else
            {
                _gridCell.SetColor(_productionUnitData.Color);
            }
        }
    }
}