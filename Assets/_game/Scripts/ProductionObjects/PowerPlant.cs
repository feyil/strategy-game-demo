using _game.Scripts.Data;
using _game.Scripts.GridComponents;

namespace _game.Scripts.ProductionObjects
{
    public class PowerPlant : ProductionObject
    {
        public PowerPlant(GridManager gridManager, ProductionData productionData, GridCell[] regionCells) : base(gridManager, productionData, regionCells)
        {
        }
    }
}