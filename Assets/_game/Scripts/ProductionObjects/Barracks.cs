using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using _game.Scripts.Interfaces;

namespace _game.Scripts.ProductionObjects
{
    public class Barracks : IGridObject
    {
        private readonly ProductionData _productionData;

        public Barracks(ProductionData productionData, GridCell[] regionCells)
        {
            _productionData = productionData;
        }

        public ProductionData GetProductionData()
        {
            return _productionData;
        }
    }
}