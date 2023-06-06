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

        public void CreateEnemies(TeamSeparator teamSeparator) =>
            InstantiateRegistered(teamSeparator);

        public void CreateGameController() =>
            InstantiateRegistered(AssetPaths.TankController);

        private void InstantiateRegistered(TeamSeparator teamSeparator)
        {
            for(int i = 0; i < teamSeparator.EnemysCount(); i ++)
            {
                ERelationship relationship = i % 2 == 0 ? ERelationship.TeamA : ERelationship.TeamB;
                SpawnPoint spawnPoint = teamSeparator.GetPoint(EPlayerType.AI, relationship);
                Vector3 point = spawnPoint.transform.position;

                GameObject enemy = _assetLoader.Instantiate(AssetPaths.Enemy, point);
                ID_Settings_CS enemyID = enemy.GetComponentInChildren<ID_Settings_CS>();
                enemyID.SetRelationship(relationship);
                EnemyDamageManagers.Add(enemy.GetComponentInChildren<DamageReciviersManager>());
                
                enemy.GetComponentInChildren<AI_Settings_CS>().WayPoint_Pack = spawnPoint.WayPointsPack;
            }
        }
    }
}