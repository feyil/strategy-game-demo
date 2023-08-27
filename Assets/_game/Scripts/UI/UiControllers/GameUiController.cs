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

        public void Show(List<ICustomScrollerData> scrollData)
        {
            m_gridManager.SpawnGrid();
            m_productionPaneController.Initialize(scrollData);
            base.Show();
        }

        public void Refresh()
        {
            m_gridManager.SpawnGrid();
        }
    }
}