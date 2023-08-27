using System.Collections;
using System.Collections.Generic;
using _game.Packages.CustomScroller;
using _game.Scripts.UI.ProductionMenu;
using _game.Scripts.UI.UiControllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
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
            UiManager.Get<GameUiController>().Show(new List<ICustomScrollerData>()
            {
                new ProductionScrollerData()
                {
                },
                new ProductionScrollerData()
                {
                },
                new ProductionScrollerData()
                {
                }
            });
        }

        [Button]
        private void RestartGame()
        {
            UiManager.Get<GameUiController>().Refresh();
        }
    }
}