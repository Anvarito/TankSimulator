using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Audio;
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
        public Action<ID_Settings_CS> OnEnemyCreate { get ; set ; }
        public int EnemiesCount { get; private set; }
        public Game_Controller_CS Controller { get; private set; }

        private readonly IStaticDataService _dataService;
        private readonly IProgressService _progress;
        private readonly IInputService _inputService;
        private Dictionary<WaypointsPackId, GameObject> _waypoints = new();


        public EnemyFactory(IAudioService audioService, IAssetLoader assetLoader, IStaticDataService dataService, IProgressService progress) : base(audioService, assetLoader)
        {
            _dataService = dataService;
            _progress = progress;
        }

        public void CreateGameController() =>
            Controller = InstantiateRegistered<Game_Controller_CS>(AssetPaths.TankController);

        public DamageReceiversManager CreateEnemy(SpawnPointConfig config)
        {
            ID_Settings_CS enemy = _assetLoader.Instantiate<ID_Settings_CS>(AssetPaths.StrykerDragon, config.Position);
            DamageReceiversManager damageReceiversManager = enemy.GetComponentInChildren<DamageReceiversManager>();

            SetupEnemy(config, enemy, damageReceiversManager);

            return damageReceiversManager;
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

        private void SetupEnemy(SpawnPointConfig config, ID_Settings_CS enemyId, DamageReceiversManager damageReceiversManager)
        {
            enemyId.SetRelationship(config.Team);
            RegisterDamageManager(damageReceiversManager, enemyId);
            EnemyDamageManagers.Add(damageReceiversManager);

            SetupEnemyWaypoints(config, enemyId.gameObject);

            OnEnemyCreate?.Invoke(enemyId);
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

        private void EnemyDestroyed(ID_Settings_CS enemyId,ID_Settings_CS killerID)
        {
            if (killerID == null)
                return;
            
            // if (killerID.PlayerType == EPlayerType.Player)
                OnEnemyDestroyed?.Invoke(killerID);
        }


        private GameObject GetWaypointPack(SpawnPointConfig config) =>
            _waypoints[config.WaypointsPackId];

        private void CreateWaypointsPack(SpawnPointConfig config) =>
            _waypoints.Add(config.WaypointsPackId,
                CreateWaypointObjects(_dataService.ForWaypoints(config.WaypointsPackId)));

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