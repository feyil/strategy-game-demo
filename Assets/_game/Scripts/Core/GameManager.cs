using System;
using System.Collections;
using System.Collections.Generic;
using _game.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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
            UiManager.Get<GameUiController>().Show();
        }

        [Button]
        private void RestartGame()
        {
            UiManager.Get<GameUiController>().Refresh();
        }
    }
}