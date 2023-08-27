using System;
using UnityEngine;

namespace _game.Scripts.Data
{
    [Serializable]
    public class ProductionUnitData
    {
        public string Name;
        public Sprite Image;
        public Vector2 Dimension;
        public int Damage;
        public int Health;
    }
}