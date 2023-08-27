using System.Collections.Generic;
using _game.Packages.CustomScroller;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    public class ProductionPaneController : MonoBehaviour
    {
        [SerializeField] private CustomScroller m_customScroller;
        [SerializeField] private CustomScrollerCellView m_customScrollerCellViewPrefab;

        public void Initialize(List<ICustomScrollerData> scrollData)
        {
            m_customScroller.Init(scrollData, m_customScrollerCellViewPrefab);
        }
    }
}
