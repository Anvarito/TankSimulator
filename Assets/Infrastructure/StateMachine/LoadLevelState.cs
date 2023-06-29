using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.TestMono;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState
        : IPayloadedState<string>
    {
        private const string PlayerInitialPoint = "PlayerInitialPoint";
        private const string EnemyInitialPoint = "EnemyInitialPoint";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;
        private readonly IFactories _factories;
        private readonly ITrashRemoveService _trashRemoveService;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;

        private List<SpawnPointConfig> _configs;
        private GamemodeConfig _modeConfig;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IProgressService progress,
            IStaticDataService dataService, IFactories factories, ITrashRemoveService trashRemoveService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progress = progress;
            _dataService = dataService;
            _factories = factories;
            _trashRemoveService = trashRemoveService;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }

        public void Enter(string payload)
        {

            _playerFactory.CleanUp();
            _enemyFactory.CleanUp();

            _sceneLoader.Load(name: payload, OnLoaded);
        }

        private void OnLoaded()
        {
            FetchModeData();
            InitGameLevel();

            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit() =>
            _configs.Clear();

        private void FetchModeData() => 
            _modeConfig = _dataService.ForMode(_progress.Progress.WorldData.ModeId);

        private void InitGameLevel()
        {
            _trashRemoveService.LaunchRemove();

            _configs = _dataService.ForLevelAndMode(_progress.Progress.WorldData.LevelId,
                _progress.Progress.WorldData.ModeId);

            List<SpawnPointConfig> enemySpawnPoints = _configs.Where(x => x.ActorType != EPlayerType.Player).ToList();
            CreateSpawners(enemySpawnPoints);

            List<SpawnPointConfig> playerPoints = _configs.Where(x => x.ActorType != EPlayerType.AI).ToList();
            CreatePlayers(playerPoints);

            _enemyFactory.CreateGameController();
        }

        private void CreateSpawners(List<SpawnPointConfig> spawnPointConfigs)
        {
            GameObject parent = new GameObject("Spawners");

            foreach (SpawnPointConfig enemyConfig in spawnPointConfigs)
            {
                GameSpawnPoint point = new GameObject("SpawnPoint").AddComponent<GameSpawnPoint>();
                point.transform.SetParent(parent.transform);
                point.Construct(_factories, enemyConfig, _modeConfig);
            }
        }

        private void CreatePlayers(List<SpawnPointConfig> spawnPointConfigs)
        {
            _playerFactory.CreatePlayers(spawnPointConfigs);
            _playerFactory.CreateTankUiSpawners(_enemyFactory.EnemyDamageManagers);
            _playerFactory.CreateHud();
        }
    }
}