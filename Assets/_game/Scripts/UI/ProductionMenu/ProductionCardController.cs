using System;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    public class ProductionCardController : MonoBehaviour
    {
        [SerializeField] private ProductionCardView m_productionCardView;

        private ProductionScrollerData _scrollerData;

        public void Init(ProductionScrollerData productionScrollerData)
        {
            _scrollerData = productionScrollerData;
            _scrollerData.AddSelectionListener(_ => { Refresh(); });
            Refresh();
        }

        private void Refresh()
        {
            var productionData = _scrollerData.ProductionData;
            if (productionData == null)
            {
                Debug.LogException(new Exception("Production Data is NULL"));
                return;
            }
            
            m_productionCardView.Render(_scrollerData.ProductionData.Image, _scrollerData.IsSelectedScroller(), OnClick);
        }

        private void OnClick()
        {
            var newState = !_scrollerData.IsSelectedScroller();
            _scrollerData.SelectScroller(newState);
            Refresh();
        }
    }
}