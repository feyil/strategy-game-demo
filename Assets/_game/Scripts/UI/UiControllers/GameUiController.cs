using System.Collections.Generic;
using _game.Packages.CustomScroller;
using _game.Scripts.Core;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using _game.Scripts.ProductionObjects;
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
            m_gridManager.SpawnGrid(GetSelection, OnCellClick);
            m_productionPaneController.Initialize(_scrollData, OnSelectionUpdate);
            m_informationPaneController.Initialize(null);
        }

        private void OnCellClick(GridCell gridCell)
        {
            var selection = GetSelection();
            if (!gridCell.IsFilled())
            {
                m_informationPaneController.Initialize(null);
            }
            else if (selection == null && gridCell.IsFilled())
            {
                //TODO get rid of from type checks to improve scalability
                var gridObject = gridCell.GetGridObject();
                var type = gridObject.GetType();
                if (type == typeof(Barracks))
                {
                    var barracks = (Barracks)gridObject;
                    m_informationPaneController.Initialize(barracks.GetProductionData());
                }
                else if (type == typeof(PowerPlant))
                {
                    var powerPlant = (PowerPlant)gridObject;
                    m_informationPaneController.Initialize(powerPlant.GetProductionData());
                }
            }
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
}