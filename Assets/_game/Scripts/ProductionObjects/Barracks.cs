using System;
using System.Collections;
using _game.Scripts.Core;
using _game.Scripts.Data;
using _game.Scripts.GridComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _game.Scripts.ProductionObjects
{
    [Serializable]
    public class Barracks : ProductionObject
    {
        private WaitForSeconds _waitForSeconds = new(3f);
        private Coroutine _spawner;

        public Barracks(GridManager gridManager, ProductionData productionData, GridCell[] regionCells) : base(
            gridManager, productionData, regionCells)
        {
            StartSpawningSoldiers();
        }

        private void StartSpawningSoldiers()
        {
            _spawner = GameManager.Instance.StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                yield return _waitForSeconds;
                SpawnSoldier();
            }
        }

        public override void Destroy()
        {
            if (_spawner != null)
            {
                GameManager.Instance.StopCoroutine(_spawner);
                _spawner = null;
            }

            base.Destroy();
        }

        private void SpawnSoldier()
        {
            var spawnCells = GetAvailableSpawnCells();
            if (spawnCells.Count == 0) return;

            var cell = spawnCells[Random.Range(0, spawnCells.Count)];
            var soliderTypeIndex = Random.Range(0, _productionData.ProductionUnitDataArray.Length);
            var soldierType = _productionData.ProductionUnitDataArray[soliderTypeIndex];
            var soldier =
                new Soldier(_gridManager, soldierType
                    , cell);
            cell.FillSpecific(soldier, soldierType.Color);
        }
    }
}