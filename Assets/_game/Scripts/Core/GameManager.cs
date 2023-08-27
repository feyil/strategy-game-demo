using System.Collections;
using System.Collections.Generic;
using _game.Packages.CustomScroller;
using _game.Scripts.Data;
using _game.Scripts.UI.ProductionMenu;
using _game.Scripts.UI.UiControllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ProductionDataSO m_productionDataSo;

        private void Awake()
        {
            InitializeAwake();
        }

        private void Start()
        {
            InitializeStart();
        }

        private void InitializeAwake()
        {
            UiManager.Instance.Initialize();
        }

        private void InitializeStart()
        {
            StartCoroutine(StartGame());
        }

        [Button]
        private IEnumerator StartGame()
        {
            yield return new WaitForEndOfFrame();

            var generateScrollData = GenerateProductionScrollData();
            UiManager.Get<GameUiController>().Show(generateScrollData);
        }

        [Button]
        private void RestartGame()
        {
            UiManager.Get<GameUiController>().Refresh();
        }

        private List<ICustomScrollerData> GenerateProductionScrollData()
        {
            var scrollData = new List<ICustomScrollerData>();

            var productionDataArray = m_productionDataSo.ProductionDataArray;
            foreach (var productionData in productionDataArray)
            {
                scrollData.Add(new ProductionScrollerData()
                {
                    ProductionData = productionData
                });
            }

            return scrollData;
        }
    }
}