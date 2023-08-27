using UnityEngine;

namespace _game.Scripts.Data
{
    [CreateAssetMenu(fileName = "ProductionDataSO", menuName = "ScriptableObjects/ProductionDataSO")]
    public class ProductionDataSO : ScriptableObject
    {
        public ProductionData[] ProductionDataArray;
    }
}