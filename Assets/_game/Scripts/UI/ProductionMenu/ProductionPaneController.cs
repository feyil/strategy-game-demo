using System;
using System.Collections.Generic;
using _game.Packages.CustomScroller;
using _game.Scripts.UI.UiControllers;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    public class ProductionPaneController : MonoBehaviour
    {
        [SerializeField] private CustomScroller m_customScroller;
        [SerializeField] private CustomScrollerCellView m_customScrollerCellViewPrefab;

        private Action<ProductionScrollerData> _onSelectionUpdate;

        public void Initialize(List<ICustomScrollerData> scrollData, Action<ProductionScrollerData> onSelectionUpdate)
        {
            _onSelectionUpdate = onSelectionUpdate;

            m_customScroller.Init(scrollData, m_customScrollerCellViewPrefab);

            m_customScroller.OnSelectionUpdated -= OnSelectionUpdate;
            m_customScroller.OnSelectionUpdated += OnSelectionUpdate;
        }

        private void OnSelectionUpdate(ICustomScrollerData scrollerData)
        {
            var selection = m_customScroller.GetSelectionAs<ProductionScrollerData>();
            _onSelectionUpdate?.Invoke(selection);
        }

        public ProductionScrollerData GetSelection()
        {
            return m_customScroller.GetSelectionAs<ProductionScrollerData>();
        }
    }
}