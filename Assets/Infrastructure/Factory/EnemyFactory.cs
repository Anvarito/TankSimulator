using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class EnemyFactory : GameFactory, IEnemyFactory
    {
        public List<DamageReciviersManager> EnemyDamageManagers { get; } = new List<DamageReciviersManager>();

        public EnemyFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public void CreateEnemies(GameObject[] at) =>
            InstantiateRegistered(at);

        public void CreateTankController() =>
            InstantiateRegistered(AssetPaths.TankController);

        private void InstantiateRegistered(GameObject[] at)
        {
            foreach (GameObject point in at)
            {
                GameObject enemy = _assetLoader.Instantiate(AssetPaths.Enemy, point.transform.position);
                EnemyDamageManagers.Add(enemy.GetComponentInChildren<DamageReciviersManager>());
                
                enemy.GetComponentInChildren<AI_Settings_CS>().WayPoint_Pack = point;
            }
        }
    }
}