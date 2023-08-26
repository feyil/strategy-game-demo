using _game.Scripts.Core;
using _game.Scripts.GridComponents;
using UnityEngine;

namespace _game.Scripts.UI
{
    public class GameUiController : UiController
    {
        [SerializeField] private GridManager m_gridManager;

        public override void Show()
        {
            m_gridManager.SpawnGrid();
            base.Show();
        }

        public void Refresh()
        {
            m_gridManager.SpawnGrid();
        }
    }
}