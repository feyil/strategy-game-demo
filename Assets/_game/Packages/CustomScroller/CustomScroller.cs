using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _game.Packages.CustomScroller
{
    public class CustomScroller : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public event Action<int> OnCellViewChange;
        public event Action OnInitialized;

        public event Action<ICustomScrollerData> OnSelectionUpdated;

        [SerializeField] private EnhancedScroller EnhanceScroller;
        [SerializeField] private bool m_isMultipleSelection;

        private List<ICustomScrollerData> _data;
        private List<CustomScrollerCellView> _cellViewList;
        public List<CustomScrollerCellView> CellViewList => _cellViewList;

        private int _activeCellViewDataIndex;
        private ICustomScrollerData _selected;


        public void Init(List<ICustomScrollerData> data, params CustomScrollerCellView[] cellViewList)
        {
            _data = data;
            _cellViewList = cellViewList.ToList();
            _selected = null;

            _activeCellViewDataIndex = 0;

            Refresh();

            OnInitialized?.Invoke();
        }

        [Button]
        public void ClearSelection()
        {
            foreach (var scrollerData in _data)
            {
                scrollerData.SelectScroller(false);
            }

            _selected = null;
        }


        public float GetNormalizedScrollPos()
        {
            return EnhanceScroller.NormalizedScrollPosition;
        }

        public void Refresh()
        {
            InitData();
            ResetScrollView();
            InitializeScrollView();

            OpenCurrentSelectedItemCellView();
        }

        public void ForceSelectFirstItem()
        {
            _data[0].SelectScroller(true);
        }

        public void ForceSelectItem(ICustomScrollerData data)
        {
            foreach (var customScrollerData in _data)
            {
                if (customScrollerData == data)
                {
                    customScrollerData.SelectScroller(true);
                    return;
                }
            }
        }

        public T GetSelectionAs<T>()
        {
            if (_selected == null) return default;
            return (T)_selected;
        }

        private void InitData()
        {
            foreach (var uData in _data)
            {
                uData.AddSelectionListener(OnSelectionUpdate);
                if (uData.IsSelectedScroller())
                {
                    _selected = uData;
                }
            }
        }

        private void OnSelectionUpdate(ICustomScrollerData data)
        {
            if (m_isMultipleSelection) return;

            var selected = _selected;
            if (_selected == data && !data.IsSelectedScroller())
            {
                _selected = null;
            }
            else if (data.IsSelectedScroller() && _selected != data)
            {
                _selected?.SelectScroller(false);
                _selected = data;
            }

            if (selected != _selected)
            {
                OnSelectionUpdated?.Invoke(_selected);
            }
        }

        public void ResetScrollView()
        {
            EnhanceScroller.Delegate = null;
            EnhanceScroller.scrollerScrolled = null;

            EnhanceScroller.ReloadData();
        }

        private void InitializeScrollView()
        {
            // Initialize Enhance Scroller
            EnhanceScroller.Delegate = this;
            EnhanceScroller.scrollerScrolled = ScrollerScrolled;

            // Instantiate CellView and Fill With Data
            EnhanceScroller.ReloadData();
        }

        private void ScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
        {
            var normalizedScrollPos = scroller.NormalizedScrollPosition;
            for (var i = 0; i < GetPageCount(); i++)
            {
                var pageCount = (float)GetPageCount();

                // Page Range

                // 4 page, 1 / 4 for each page slice
                // [0, 1/4]
                // [1/4, 2/4]
                // [2/4, 3/4]
                // [3/4, 1]

                var lowerBound = i / pageCount;
                var upperBound = (i + 1) / pageCount;
                // Page Range

                if (normalizedScrollPos >= lowerBound && normalizedScrollPos < upperBound)
                {
                    // already dont waste cpu
                    if (_activeCellViewDataIndex == i) return;

                    OnCellViewChanged(i);
                }
            }
        }

        private void OpenCurrentSelectedItemCellView()
        {
            if (_selected != null)
            {
                var index = GetItemCellViewDataIndex(_selected);
                EnhanceScroller.JumpToDataIndex(index, jumpComplete: () => OnCellViewChanged(index));
            }
            else if (_data.Count > 0)
            {
                EnhanceScroller.JumpToDataIndex(0, jumpComplete: () => OnCellViewChanged(0));
            }
        }

        public CustomScrollerCellView GetSelectedCellView(int offset = 0, int defaultIndex = 0)
        {
            if (_selected == null)
            {
                return EnhanceScroller.GetCellViewAtDataIndex(defaultIndex) as CustomScrollerCellView;
            }

            var index = GetItemCellViewDataIndex(_selected);
            if (EnhanceScroller.NumberOfCells < index + offset) return null;
            EnhanceScroller.JumpToDataIndex(index + offset);
            var cellViewAtDataIndex = EnhanceScroller.GetCellViewAtDataIndex(index + offset);
            return cellViewAtDataIndex as CustomScrollerCellView;
        }

        private int GetItemCellViewDataIndex(ICustomScrollerData scrollerData, int defaultReturn = 0)
        {
            var data = _data;

            //Page Check
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if (item == scrollerData)
                {
                    return i / _cellViewList
                        .Find(view =>
                            view.GetType() == scrollerData.GetCellViewType() ||
                            view.GetType().IsSubclassOf(scrollerData.GetCellViewType())).GetCellViewItemCount();
                }
            }

            return defaultReturn;
        }

        private void OnCellViewChanged(int activeCellViewDataIndex)
        {
            _activeCellViewDataIndex = activeCellViewDataIndex;
            OnCellViewChange?.Invoke(activeCellViewDataIndex);
            // var cellViewAtDataIndex = EnhanceScroller.GetCellViewAtDataIndex(activeCellViewDataIndex);
            // if (cellViewAtDataIndex != null)
            // {
            //     OnCellViewChange?.Invoke((customScrollerCellView) cellViewAtDataIndex);
            // }

            // Debug.Log($"Page Updated {activeCellViewDataIndex}");
        }

        public void SetScrollerActivity(bool isActive)
        {
            var scrollRect = EnhanceScroller.ScrollRect;
            if (scrollRect == null)
            {
                Debug.LogWarning("Scroll rect is null");
                return;
            }

            scrollRect.enabled = isActive;
        }

        private int GetPageCount()
        {
            var data = _data;

            var tmp = data.Count / (float)_cellViewList[0].GetCellViewItemCount();
            var pageCount = (int)Math.Ceiling(tmp);

            return pageCount;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return GetPageCount();
        }

        public bool IsScrollerDataEmpty()
        {
            return _data.IsNullOrEmpty();
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return GetCellViewWithDataIndex(dataIndex).GetCellViewSize();
        }

        private CustomScrollerCellView GetCellViewWithDataIndex(int dataIndex)
        {
            return _cellViewList.Find(view =>
                view.GetType() == _data[dataIndex].GetCellViewType() ||
                view.GetType().IsSubclassOf(_data[dataIndex].GetCellViewType()));
        }

        public CustomScrollerCellView GetCellViewAtDataIndex(int index)
        {
            return EnhanceScroller.GetCellViewAtDataIndex(index) as CustomScrollerCellView;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellViewPrefab = GetCellViewWithDataIndex(dataIndex);
            var cellView = scroller.GetCellView(cellViewPrefab) as CustomScrollerCellView;

            var tmpDataList = new List<ICustomScrollerData>();
            var itemCount = cellViewPrefab.GetCellViewItemCount();

            var startIndex = itemCount * cellIndex;
            var endIndex = itemCount * (cellIndex + 1);
            for (var j = startIndex; j < endIndex; j++)
            {
                if (j < _data.Count)
                {
                    tmpDataList.Add(_data[j]);
                }
            }

            if (cellView != null)
            {
                cellView.Init(tmpDataList);
            }
            else
            {
                Debug.LogError("GetCellView sectionGridCellView is null");
            }

            return cellView;
        }

        [Button]
        public float GetScrollPos()
        {
            Debug.Log(EnhanceScroller.ScrollPosition);
            return EnhanceScroller.ScrollPosition;
        }

        [Button]
        public void SetScrollPos(float scroll)
        {
            var inverseLerp = (int)(Mathf.InverseLerp(0, EnhanceScroller.ScrollSize, scroll) *
                                    EnhanceScroller.NumberOfCells);
            EnhanceScroller.JumpToDataIndex(inverseLerp - 1);
            EnhanceScroller.ScrollPosition = scroll;
        }

        [Button]
        public void JumpDataIndex(int index, float scrollerOffset = 0.5f, float tweenTime = 0.5f,
            EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.easeOutBounce, Action onComplete = null)
        {
            EnhanceScroller.JumpToDataIndex(index, scrollerOffset, 0f, true, tweenType, tweenTime, onComplete);
        }
    }
}