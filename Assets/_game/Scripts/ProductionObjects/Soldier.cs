using System.Collections;
using _game.Scripts.Core;
using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using _game.Scripts.Utility;
using UnityEngine;

namespace _game.Scripts.ProductionObjects
{
    public class Soldier : IGridObject
    {
        private readonly ProductionUnitData _productionUnitData;
        private GridCell _gridCell;
        private bool _isSelected;
        private readonly GridManager _gridManager;
        private Coroutine _traverseAnimation;

        public Soldier(GridManager gridManager, ProductionUnitData productionUnitData, GridCell gridCell)
        {
            _gridManager = gridManager;
            _productionUnitData = productionUnitData;
            _gridCell = gridCell;
        }

        public void Destroy()
        {
            if (_traverseAnimation != null)
            {
                GameManager.Instance.StopCoroutine(_traverseAnimation);    
            }
        }

        public void Hit(float damage)
        {
        }

        public void Move(GridCell gridCell)
        {
            var path = AStarPathfinder.GetPath(_gridManager, _gridCell.GetCord(), gridCell.GetCord());
            _traverseAnimation = GameManager.Instance.StartCoroutine(TraverseAnimation(path));
        }

        private IEnumerator TraverseAnimation(GridCell[] path)
        {
            foreach (var gridCell in path)
            {
                yield return new WaitForSeconds(0.2f);
                _gridCell.Fill(null);
                _gridCell = gridCell;
                gridCell.FillSpecific(this, _productionUnitData.Color);
            }
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