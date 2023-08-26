using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.Scripts.GridComponents
{
    public class GridCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField, ReadOnly] private Vector2 m_cord;

        [Button]
        public void Initialize(Vector2 cord, Vector2 localPosition)
        {
            m_cord = cord;
            name = $"x_{m_cord.x}_y_{m_cord.y}";
            m_rectTransform.anchoredPosition = localPosition;
        }

        [Button]
        public Vector2 GetSize()
        {
            var rect = m_rectTransform.rect;
            return new Vector2(rect.width, rect.height);
        }

        public string GetIndex()
        {
            return name;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.green;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}