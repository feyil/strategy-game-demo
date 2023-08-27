using _game.Scripts.Data;
using UnityEngine;

namespace _game.Scripts.UI.ProductionMenu
{
    public class InformationPaneController : MonoBehaviour
    {
        [SerializeField] private InformationPaneView m_infoPaneView;

        public void Initialize(ProductionData productionData)
        {
            if (productionData == null)
            {
                m_infoPaneView.Render();
            }
            else
            {
                m_infoPaneView.Render(productionData.Name, productionData.Image, "Production Unit",
                    productionData.ProductionUnitDataArray);
            }
        }
    }
}