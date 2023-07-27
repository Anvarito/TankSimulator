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
        public Action<ID_Settings_CS> OnEnemyCreate { get; set; }
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
            var path = config.Team == ERelationship.TeamA ? GetRandomAllyPath() : GetRandomEnemyPath();
            ID_Settings_CS enemy = _assetLoader.Instantiate<ID_Settings_CS>(path, config.Position);
            DamageReceiversManager damageReceiversManager = enemy.GetComponentInChildren<DamageReceiversManager>();

            SetupEnemy(config, enemy, damageReceiversManager);

            return damageReceiversManager;
        }

        private string GetRandomEnemyPath()
        {
            int random = UnityEngine.Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    return AssetPaths.StrykerDragon;
                case 1:
                    return AssetPaths.Bradley;
                default:
                    return AssetPaths.BMP2;
            }

        }

        private string GetRandomAllyPath()
        {
            int random = UnityEngine.Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    return AssetPaths.Nona9S;
                default:
                    return AssetPaths.Vena;
            }

        }

        public override void CleanUp()
        {
            base.CleanUp();

            foreach (var i in EnemyDamageManagers)
            {
                if (i == null)
                    continue;

                Object.Destroy(i.transform.root.gameObject);
            }

            EnemiesCount = 0;
            EnemyDamageManagers.Clear();
            _waypoints.Clear();
        }

        private void SetupEnemy(SpawnPointConfig config, ID_Settings_CS enemyId, DamageReceiversManager damageReceiversManager)
        {
            enemyId.SetRelationship(config.Team);
            RegisterDamageManager(damageReceiversManager, enemyId);
            EnemyDamageManagers.Add(damageReceiversManager);

            SetupEnemyWaypoints(config, enemyId.GetComponentInChildren<AI_Settings_CS>());
            SetupEnemyAgression(enemyId.GetComponentInChildren<AI_Settings_CS>());

            OnEnemyCreate?.Invoke(enemyId);
        }

        private void SetupEnemyAgression(AI_Settings_CS AIsettings)
        {
            AIsettings.No_Attack = _progress.Progress.WorldData.ModeId == Services.StaticData.Gamemodes.GamemodeId.Training;
        }

        private void SetupEnemyWaypoints(SpawnPointConfig config, AI_Settings_CS AIsettings)
        {
            if (!_waypoints.ContainsKey(config.WaypointsPackId))
                CreateWaypointsPack(config);

            if (_progress.Progress.WorldData.ModeId != Services.StaticData.Gamemodes.GamemodeId.Training)
                AIsettings.WayPoint_Pack = GetWaypointPack(config);
            else
                AIsettings.WayPoint_Pack = null;
        }

        private void RegisterDamageManager(DamageReceiversManager enemyDamageManager, ID_Settings_CS settingsCs)
        {
            if (_progress.Progress.WorldData.Teams.All(x => x != settingsCs.Relationship))
            {
                EnemiesCount++;
                enemyDamageManager.OnTankDestroyed.AddListener(EnemyDestroyed);
            }
        }

        private void EnemyDestroyed(ID_Settings_CS enemyId, ID_Settings_CS killerID)
        {
            OnEnemyDestroyed?.Invoke(killerID);
        }


        private GameObject GetWaypointPack(SpawnPointConfig config) =>
            _waypoints[config.WaypointsPackId];

        private void CreateWaypointsPack(SpawnPointConfig config) =>
            _waypoints.Add(config.WaypointsPackId,
                CreateWaypointObjects(_dataService.ForWaypoints(config.WaypointsPackId)));

        private GameObject CreateWaypointObjects(WaypointPackConfig waypointPack)
        {
            if (waypointPack == null)
                return null;

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