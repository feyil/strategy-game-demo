using System;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Scripts.UI.ProductionMenu
{
    public class ProductionCardView : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        [SerializeField] private Button m_selfButton;

        [SerializeField] private Image m_selectionFrame;

        private Action _onClickListener;

        public void Render(Sprite sprite, bool isSelected, Action onClickListener)
        {
            m_image.sprite = sprite;
            _onClickListener = onClickListener;
            
            m_selfButton.onClick.RemoveAllListeners();
            m_selfButton.onClick.AddListener(OnClick);

            m_selectionFrame.enabled = isSelected;
        }

        private void OnClick()
        {
            _onClickListener?.Invoke();
        }
    }
}