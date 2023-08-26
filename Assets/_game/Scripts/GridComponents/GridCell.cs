using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.Scripts.GridComponents
{
    public class GridCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private Image m_image;
        [SerializeField, ReadOnly] private Vector2 m_cord;

        private GridCellEvents _gridCellEvents;

        [Button]
        public void Initialize(Vector2 cord, Vector2 localPosition, GridCellEvents gridCellEvents)
        {
            m_cord = cord;
            name = GetIndex((int)cord.x, (int)cord.y);
            m_rectTransform.anchoredPosition = localPosition;
            _gridCellEvents = gridCellEvents;
        }

        public static string GetIndex(int x, int y)
        {
            return $"x_{x}_y_{y}";
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

        public Vector2 GetCord()
        {
            return m_cord;
        }

        public void SetColor(Color color)
        {
            m_image.color = color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _gridCellEvents.OnCellEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _gridCellEvents.OnCellExit?.Invoke(this);
        }
    }
}