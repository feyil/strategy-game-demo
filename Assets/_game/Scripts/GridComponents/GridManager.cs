using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.GridComponents
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private RectTransform m_container;
        [SerializeField] private Vector2 m_spacing;
        [SerializeField] private GridCell m_gridCellPrefab;

        private Dictionary<string, GridCell> _currentGrid;

        [Button]
        public void SpawnGrid()
        {
            SpawnGrid(m_container);
        }

        private void SpawnGrid(RectTransform contentArea)
        {
            _currentGrid = new Dictionary<string, GridCell>();

            var contentAreaRect = contentArea.rect;
            var width = contentAreaRect.width;
            var height = contentAreaRect.height;

            var size = m_gridCellPrefab.GetSize();
            var cellWidth = size.x + m_spacing.x;
            var cellHeight = size.y + m_spacing.y;

            var columnCount = (int)(width / cellWidth);
            var rowCount = (int)(height / cellHeight);

            for (var currentRow = 0; currentRow < rowCount; currentRow++)
            {
                for (var currentColumn = 0; currentColumn < columnCount; currentColumn++)
                {
                    var gridCell = Instantiate(m_gridCellPrefab, contentArea.transform);

                    var cord = new Vector2(currentRow, currentColumn);
                    var localPosition = new Vector2(currentColumn * cellWidth,
                        -currentRow * cellHeight);

                    gridCell.Initialize(cord, localPosition);

                    var index = gridCell.GetIndex();
                    _currentGrid.Add(index, gridCell);
                }
            }
        }

        [Button]
        public void CleanUp()
        {
            foreach (var value in _currentGrid.Values)
            {
                if (Application.isPlaying) Destroy(value.gameObject);
                else DestroyImmediate(value.gameObject);
            }
        }
    }
}