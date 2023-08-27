using System.Collections.Generic;
using _game.Packages.CustomScroller;
using _game.Scripts.Core;
using _game.Scripts.GridComponents;
using _game.Scripts.UI.ProductionMenu;
using UnityEngine;

namespace _game.Scripts.UI.UiControllers
{
    public class GameUiController : UiController
    {
        [SerializeField] private GridManager m_gridManager;
        [SerializeField] private ProductionPaneController m_productionPaneController;
        [SerializeField] private InformationPaneController m_informationPaneController;

        private List<ICustomScrollerData> _scrollData;

        public void Show(List<ICustomScrollerData> scrollData)
        {
            _scrollData = scrollData;
            Refresh();

            base.Show();
        }

        public void Refresh()
        {
            m_gridManager.SpawnGrid(GetSelection);
            m_productionPaneController.Initialize(_scrollData, OnSelectionUpdate);
            m_informationPaneController.Initialize(null);
        }

        private void OnSelectionUpdate(ProductionScrollerData productionScrollerData)
        {
            m_informationPaneController.Initialize(productionScrollerData?.ProductionData);
        }

        private IProductionSelection GetSelection()
        {
            return m_productionPaneController.GetSelection();
        }
    }

    public interface IProductionSelection
    {
        Vector2 GetDimensions();
        void PlaceObject();
    }
}