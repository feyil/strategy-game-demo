using System;
using _game.Packages.CustomScroller;
using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using _game.Scripts.ProductionObjects;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    [Serializable]
    public class ProductionScrollerData : ICustomScrollerData, IProductionSelection
    {
        public ProductionData ProductionData;

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

        public Vector2 GetDimensions()
        {
            return ProductionData.Dimension;
        }

        public IGridObject CreateGridObject(GridCell[] regionCells)
        {
            switch (ProductionData.Id)
            {
                case "barracks":
                    return new Barracks(ProductionData, regionCells);
                case "power_plant":
                    return new PowerPlant(ProductionData, regionCells);
            }

            return null;
        }
    }
}