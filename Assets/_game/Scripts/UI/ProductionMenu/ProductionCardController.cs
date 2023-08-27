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
            m_productionCardView.Render(_scrollerData.ProductionSprite, _scrollerData.IsSelectedScroller(), OnClick);
        }

        private void OnClick()
        {
            var newState = !_scrollerData.IsSelectedScroller();
            _scrollerData.SelectScroller(newState);
            Refresh();
        }
    }
}