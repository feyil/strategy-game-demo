using System;

namespace _game.Packages.CustomScroller
{
    public interface ICustomScrollerData
    {
        void SelectScroller(bool state);
        Type GetCellViewType();
        bool IsSelectedScroller();
        void AddSelectionListener(Action<ICustomScrollerData> listener);
    }
}