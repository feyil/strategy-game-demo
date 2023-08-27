using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;

namespace _game.Scripts.ProductionObjects
{
    public class PowerPlant : IGridObject
    {
        private readonly ProductionData _productionData;

        public PowerPlant(ProductionData productionData, GridCell[] regionCells)
        {
            _productionData = productionData;
        }

        public ProductionData GetProductionData()
        {
            return _productionData;
        }
    }
}