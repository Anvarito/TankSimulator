using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.StaticData.Waypoints;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory
{
    public class EnemyFactory : GameFactory, IEnemyFactory
    {
        public List<DamageReceiversManager> EnemyDamageManagers { get; } = new List<DamageReceiversManager>();
        public Action<ID_Settings_CS> OnEnemyDestroyed { get; set; }
        public int EnemiesCount { get; private set; }

        private readonly IStaticDataService _dataService;
        private readonly IProgressService _progress;
        private readonly IInputService _inputService;
        private Dictionary<WaypointsPackId, GameObject> _waypoints = new();


        public EnemyFactory(IAssetLoader assetLoader, IStaticDataService dataService, IProgressService progress) : base(assetLoader)
        {
            _dataService = dataService;
            _progress = progress;
        }

        public void CreateGameController() =>
            InstantiateRegistered(AssetPaths.TankController);

        public void CreateEnemy(SpawnPointConfig config)
        {
            GameObject enemy = _assetLoader.Instantiate(AssetPaths.Enemy, config.Position);
            SetupEnemy(config, enemy);
        }

        public override void CleanUp()
        {
            base.CleanUp();

            foreach (var i in EnemyDamageManagers)
            {
                Object.Destroy(i.transform.root.gameObject);
            }

            EnemyDamageManagers.Clear();
            _waypoints.Clear();
        }

        private void SetupEnemy(SpawnPointConfig config, GameObject enemy)
        {
            ID_Settings_CS enemyID = enemy.GetComponentInChildren<ID_Settings_CS>();
            enemyID.SetRelationship(config.Team);
            DamageReceiversManager damageReceiversManager = enemy.GetComponentInChildren<DamageReceiversManager>();
            RegisterDamageManager(damageReceiversManager, enemyID);
            EnemyDamageManagers.Add(damageReceiversManager);

            SetupEnemyWaypoints(config, enemy);
        }

        private void SetupEnemyWaypoints(SpawnPointConfig config, GameObject enemy)
        {
            if (!_waypoints.ContainsKey(config.WaypointsPackId))
                CreateWaypointsPack(config);

            enemy.GetComponentInChildren<AI_Settings_CS>().WayPoint_Pack = GetWaypointPack(config);
        }

        private void RegisterDamageManager(DamageReceiversManager enemyDamageManager, ID_Settings_CS settingsCs)
        {
            if (_progress.Progress.WorldData.Teams.All(x => x != settingsCs.Relationship))
            {
                EnemiesCount++;
                enemyDamageManager.OnTankDestroyed.AddListener(EnemyDestroyed);
            }
        }

        private void EnemyDestroyed(ID_Settings_CS killerID)
        {
            if (killerID.PlayerType == EPlayerType.Player)
                OnEnemyDestroyed?.Invoke(killerID);
        }


        private GameObject GetWaypointPack(SpawnPointConfig config) =>
            _waypoints[config.WaypointsPackId];

        private void CreateWaypointsPack(SpawnPointConfig config) =>
            _waypoints.Add(config.WaypointsPackId, CreateWaypointObjects(_dataService.ForWaypoints(config.WaypointsPackId)));

        private GameObject CreateWaypointObjects(WaypointPackConfig waypointPack)
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