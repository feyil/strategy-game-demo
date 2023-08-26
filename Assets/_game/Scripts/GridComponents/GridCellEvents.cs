using System;

namespace _game.Scripts.GridComponents
{
    [Serializable]
    public class GridCellEvents
    {
        public Action<GridCell> OnCellEnter;
        public Action<GridCell> OnCellExit;
        public Action<GridCell> OnCellClick;
    }
}