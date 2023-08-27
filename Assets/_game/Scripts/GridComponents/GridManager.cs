using System;
using System.Collections.Generic;
using _game.Scripts.Interfaces;
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
        private Func<IProductionSelection> _getSelection;
        private Action<GridCell> _onCellClick;

        [Button]
        public void SpawnGrid(Func<IProductionSelection> getSelection, Action<GridCell> onCellClick)
        {
            _getSelection = getSelection;
            _onCellClick = onCellClick;

            CleanUp();
            SpawnGrid(m_container);
        }

        private Vector2 GetSelectionDimensions()
        {
            var selection = _getSelection?.Invoke();
            if (selection == null) return Vector2.zero;

            return selection.GetDimensions();
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

                    gridCell.Initialize(cord, localPosition, new GridCellEvents()
                    {
                        OnCellEnter = OnCellEnter,
                        OnCellExit = OnCellExit,
                        OnCellClick = OnCellClick
                    });

                    var index = gridCell.GetIndex();
                    _currentGrid.Add(index, gridCell);
                }
            }
        }

        [Button]
        public void CleanUp()
        {
            if (_currentGrid == null) return;
            foreach (var value in _currentGrid.Values)
            {
                if (Application.isPlaying) Destroy(value.gameObject);
                else DestroyImmediate(value.gameObject);
            }
        }

        private void OnCellEnter(GridCell gridCell)
        {
            var startCord = gridCell.GetCord();

            var regionCells = GetRegionCells(startCord, GetSelectionDimensions());
            var isAvailable = IsAvailable(regionCells);
            if (isAvailable)
            {
                ColorARegion(regionCells, Color.green);
            }
            else
            {
                ColorARegion(regionCells, Color.red);
            }
        }

        private void OnCellExit(GridCell gridCell)
        {
            var startCord = gridCell.GetCord();

            var regionCells = GetRegionCells(startCord, GetSelectionDimensions());
            ColorARegion(regionCells, Color.white);
        }

        private void OnCellClick(GridCell gridCell)
        {
            var startCord = gridCell.GetCord();

            var regionCells = GetRegionCells(startCord, GetSelectionDimensions());
            var isAvailable = IsAvailable(regionCells);
            if (isAvailable)
            {
                PlaceObject(regionCells);
            }

            _onCellClick?.Invoke(gridCell);
        }

        private void PlaceObject(GridCell[] regionCells)
        {
            var selection = _getSelection?.Invoke();
            if (selection == null) return;

            var gridObject = selection.CreateGridObject(this, regionCells);
            foreach (var regionCell in regionCells)
            {
                regionCell.Fill(gridObject);
            }
        }

        public GridCell GetCell(Vector2 cord)
        {
            return GetCell(cord.x, cord.y);
        }

        private GridCell GetCell(float x, float y)
        {
            var index = GridCell.GetIndex((int)x, (int)y);
            _currentGrid.TryGetValue(index, out var cell);

            return cell;
        }

        private GridCell[] GetRegionCells(Vector2 startCord, Vector2 selectionSize)
        {
            var regionCells = new GridCell[(int)(selectionSize.x * selectionSize.y)];

            var counter = 0;
            for (var i = 0; i < selectionSize.x; i++)
            {
                var shiftX = startCord.x + i;
                for (var j = 0; j < selectionSize.y; j++)
                {
                    var shiftY = startCord.y + j;
                    var cell = GetCell(shiftX, shiftY);
                    regionCells[counter] = cell;
                    counter++;
                }
            }

            return regionCells;
        }

        private void ColorARegion(GridCell[] regionCells, Color color)
        {
            foreach (var regionCell in regionCells)
            {
                if (regionCell == null || regionCell.IsFilled()) continue;
                regionCell.SetColor(color);
            }
        }

        private bool IsAvailable(GridCell[] regionCells)
        {
            foreach (var regionCell in regionCells)
            {
                var isOutsideRangeOrFilled = regionCell == null || regionCell.IsFilled();
                if (isOutsideRangeOrFilled)
                {
                    return false;
                }
            }

            return true;
        }
    }
}