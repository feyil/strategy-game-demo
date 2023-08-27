using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace _game.Packages.CustomScroller
{
    public abstract class CustomScrollerCellView : EnhancedScrollerCellView
    {
        private float _cellSize = Single.MinValue;
        public abstract void Init(List<ICustomScrollerData> data);
        
        public virtual int GetCellViewItemCount()
        {
            return 1;
        }
        
        public virtual float GetCellViewSize()
        {
            if (Math.Abs(_cellSize - Single.MinValue) < 5)
            {
                _cellSize = GetComponent<RectTransform>().sizeDelta.y;
            }

            return _cellSize;
        }
        
        private void Reset()
        {
            cellIdentifier = name;
        }
    }
}