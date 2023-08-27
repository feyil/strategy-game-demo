using System.Collections.Generic;
using _game.Scripts.GridComponents;

namespace _game.Scripts.Interfaces
{
    public interface IGridObject
    {
        public void Destroy();
        public void Hit(float damage);

        public float GetHealth();

        public void Move(GridCell gridCell);

        public List<GridCell> GetAvailableSpawnCells();
    }
}