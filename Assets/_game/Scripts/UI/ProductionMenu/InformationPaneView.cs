using _game.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Scripts.UI.ProductionMenu
{
    public class InformationPaneView : MonoBehaviour
    {
        [SerializeField] private GameObject m_productionContainer;
        [SerializeField] private GameObject m_productionUnitContainer;

        [SerializeField] private TextMeshProUGUI m_productionTittle;
        [SerializeField] private TextMeshProUGUI m_productionUnitTitle;

        [SerializeField] private Image m_productionImage;
        [SerializeField] private Image[] m_productionUnitImageArray;

        public void Render()
        {
            m_productionContainer.SetActive(false);
            m_productionUnitContainer.SetActive(false);
        }

        public void Render(string productionTitle, Sprite productionSprite, string productionUnitTitle = null,
            ProductionUnitData[] productionUnitDataArray = null)
        {
            m_productionContainer.SetActive(true);

            m_productionTittle.SetText(productionTitle);
            m_productionImage.sprite = productionSprite;

            m_productionUnitContainer.SetActive(productionUnitDataArray != null && productionUnitDataArray.Length != 0);

            m_productionUnitTitle.SetText(productionUnitTitle);
            for (var index = 0; index < m_productionUnitImageArray.Length; index++)
            {
                var productionUnitImage = m_productionUnitImageArray[index];
                if (productionUnitDataArray != null && index < productionUnitDataArray.Length)
                {
                    productionUnitImage.sprite = productionUnitDataArray[index].Image;
                    productionUnitImage.gameObject.SetActive(true);
                }
                else
                {
                    productionUnitImage.gameObject.SetActive(false);
                }
            }
        }
    }
}