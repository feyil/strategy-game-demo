using System;
using UnityEngine;

namespace _game.Scripts.Data
{
    [Serializable]
    public class ProductionData
    {
        public string Name;
        public Sprite Image;
        public Vector2 Dimension;
        public int Health;
        public ProductionUnitData[] ProductionUnitDataArray;
    }
}