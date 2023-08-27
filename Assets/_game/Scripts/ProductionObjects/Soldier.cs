using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using UnityEngine;

namespace _game.Scripts.ProductionObjects
{
    public class Soldier : IGridObject
    {
        private readonly ProductionUnitData _productionUnitData;
        private readonly GridCell _gridCell;
        private bool _isSelected;

        public Soldier(ProductionUnitData productionUnitData, GridCell gridCell)
        {
            _productionUnitData = productionUnitData;
            _gridCell = gridCell;
        }
        
        public void Destroy()
        {
            
        }

        public void Hit(float damage)
        {
            
        }

        public void Move(GridCell gridCell)
        {
            Select(false);
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