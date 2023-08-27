using System.Collections.Generic;
using _game.Packages.CustomScroller;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    public class ProductionScrollerRow : CustomScrollerCellView
    {
        [SerializeField] private ProductionCardController[] m_productionCardControllerArray;

        public override void Init(List<ICustomScrollerData> data)
        {
            for (var index = 0; index < m_productionCardControllerArray.Length; index++)
            {
                var productionCardController = m_productionCardControllerArray[index];
                if (index < data.Count)
                {
                    productionCardController.gameObject.SetActive(true);
                    
                    var scrollData = (ProductionScrollerData)data[index];
                    productionCardController.Init(scrollData);
                }
                else
                {
                    productionCardController.gameObject.SetActive(false);
                }
            }
        }

        public override int GetCellViewItemCount()
        {
            return 2;
        }

        public override float GetCellViewSize()
        {
            return 200;
        }
    }
}