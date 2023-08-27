using _game.Scripts.GridComponents;
using UnityEngine;

namespace _game.Scripts.Interfaces
{
    public interface IProductionSelection
    {
        Vector2 GetDimensions();
        IGridObject CreateGridObject(GridCell[] regionCells);
    }
}