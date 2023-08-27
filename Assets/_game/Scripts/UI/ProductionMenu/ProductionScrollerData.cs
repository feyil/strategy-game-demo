using System;
using _game.Packages.CustomScroller;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    [Serializable]
    public class ProductionScrollerData : ICustomScrollerData
    {
        public Sprite ProductionSprite;

        private bool _isSelected;
        private event Action<ICustomScrollerData> _onSelected;

        public void SelectScroller(bool state)
        {
            _isSelected = state;
            _onSelected?.Invoke(this);
        }

        public Type GetCellViewType()
        {
            return typeof(ProductionScrollerRow);
        }

        public bool IsSelectedScroller()
        {
            return _isSelected;
        }

        public void AddSelectionListener(Action<ICustomScrollerData> listener)
        {
            _onSelected -= listener;
            _onSelected += listener;
        }
    }
}