using System.Collections.Generic;
using _game.Packages.CustomScroller;
using _game.Scripts.Core;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;
using _game.Scripts.ProductionObjects;
using _game.Scripts.UI.ProductionMenu;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Scripts.UI.UiControllers
{
    public class GameUiController : UiController
    {
        [SerializeField] private Button m_restartButton;
        [SerializeField] private Button m_exitButton;
        [SerializeField] private GridManager m_gridManager;
        [SerializeField] private ProductionPaneController m_productionPaneController;
        [SerializeField] private InformationPaneController m_informationPaneController;

        private List<ICustomScrollerData> _scrollData;
        private Soldier _selectedSoldier;

        public void Show(List<ICustomScrollerData> scrollData)
        {
            m_restartButton.onClick.RemoveAllListeners();
            m_restartButton.onClick.AddListener(() =>
            {
                GameManager.Instance.RestartGame();
            });
            
            m_exitButton.onClick.RemoveAllListeners();
            m_exitButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ExitGame();
            });

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

        private void OnCellClick(GridCell gridCell, int button)
        {
            // 0 => leftButton
            // 1 => rightButton
            // 2 => middleButton

            if (button == 0)
            {
                OnLeftButtonClick(gridCell);
            }
            else if (button == 1)
            {
                OnRightButtonClick(gridCell);
            }
        }

        private void OnRightButtonClick(GridCell gridCell)
        {
            if (_selectedSoldier != null)
            {
                _selectedSoldier.Move(gridCell);
                ResetSoldierSelection();
            }
        }

        private void OnLeftButtonClick(GridCell gridCell)
        {
            var selection = GetSelection();
            if (!gridCell.IsFilled())
            {
                m_informationPaneController.Initialize(null);
                ResetSoldierSelection();
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
                    ResetSoldierSelection();
                }
                else if (type == typeof(PowerPlant))
                {
                    var powerPlant = (PowerPlant)gridObject;
                    m_informationPaneController.Initialize(powerPlant.GetProductionData());
                    ResetSoldierSelection();
                }
                else if (type == typeof(Soldier))
                {
                    var soldier = (Soldier)gridObject;
                    var newSelectionState = !soldier.IsSelected();
                    soldier.Select(newSelectionState);
                    if (newSelectionState && _selectedSoldier != soldier)
                    {
                        _selectedSoldier?.Select(false);
                        _selectedSoldier = soldier;
                    }
                }
            }
        }

        private void ResetSoldierSelection()
        {
            _selectedSoldier?.Select(false);
            _selectedSoldier = null;
        }

        private void OnSelectionUpdate(ProductionScrollerData productionScrollerData)
        {
            m_informationPaneController.Initialize(productionScrollerData?.ProductionData);
            ResetSoldierSelection();
        }

        private IProductionSelection GetSelection()
        {
            return m_productionPaneController.GetSelection();
        }
    }
}