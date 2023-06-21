using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.StaticData.Waypoints;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class EnemyFactory : GameFactory, IEnemyFactory
    {
        private readonly IStaticDataService _dataService;
        public List<DamageReciviersManager> EnemyDamageManagers { get; } = new List<DamageReciviersManager>();

        public EnemyFactory(IAssetLoader assetLoader, IStaticDataService dataService) : base(assetLoader)
        {
            _dataService = dataService;
        }

        public void CreateGameController() =>
            InstantiateRegistered(AssetPaths.TankController);

        public void CreateEnemy(SpawnPointConfig config)
        {
            GameObject enemy = _assetLoader.Instantiate(AssetPaths.Enemy, config.Position);
            ID_Settings_CS enemyID = enemy.GetComponentInChildren<ID_Settings_CS>();
            enemyID.SetRelationship(config.Team);
            EnemyDamageManagers.Add(enemy.GetComponentInChildren<DamageReciviersManager>());
                
            enemy.GetComponentInChildren<AI_Settings_CS>().WayPoint_Pack =
                CreateWaypoints(_dataService.ForWaypoints(config.WaypointsPackId));
        }

        public override void CleanUp()
        {
            base.CleanUp();

            foreach(var i in EnemyDamageManagers)
            {
                GameObject.Destroy(i.transform.root.gameObject);
            }

            EnemyDamageManagers.Clear();
        }

        private GameObject CreateWaypoints(WaypointPackConfig waypointPack)
        {
            string name = Enum.GetName(typeof(WaypointsPackId), waypointPack.PackId);

            GameObject packObject = new GameObject(name);

            foreach (var point in waypointPack.Points.Select((value, index) => new { position = value, index }))
            {
                Transform pointObject = new GameObject("Waypoint " + point.index).transform;
                pointObject.position = point.position;
                pointObject.SetParent(packObject.transform);
            }

            return packObject;
        }
    }
}