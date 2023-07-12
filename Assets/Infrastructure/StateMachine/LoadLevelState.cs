using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Components;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Score;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.SpawnPoints;
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
        private readonly IAudioService _audioService;
        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;
        private readonly IFactories _factories;
        private readonly ITrashRemoveService _trashRemoveService;
        private readonly IScoreCounter _scoreCounter;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;

        private List<SpawnPointConfig> _configs;
        private GamemodeConfig _modeConfig;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IAudioService audioService, IProgressService progress,
            IStaticDataService dataService, IFactories factories, ITrashRemoveService trashRemoveService, IScoreCounter scoreCounter)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _audioService = audioService;
            _progress = progress;
            _dataService = dataService;
            _factories = factories;
            _trashRemoveService = trashRemoveService;
            _scoreCounter = scoreCounter;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }

        public void Enter(string levelName)
        {
            _scoreCounter.CleanUp();
            _playerFactory.CleanUp();
            _enemyFactory.CleanUp();

                _sceneLoader.Load(name: levelName, OnLoaded, OnProgress);
            
        }

        private void OnProgress(float progress)
        {
            Debug.Log(progress);
        }

        public void Exit() =>
            _configs.Clear();

        private void OnLoaded()
        {
            FetchModeData();
            InitGameLevel();

            _audioService.PlayMusic(MusicId.Test);

            _gameStateMachine.Enter<GameLoopState>();
        }


        private void FetchModeData() =>
            _modeConfig = _dataService.ForMode(_progress.Progress.WorldData.ModeId);

        private void InitGameLevel()
        {
            _trashRemoveService.LaunchRemove();
            CreatePlayersAndEnemySpawners();
            _enemyFactory.CreateGameController();
        }

        private void CreatePlayersAndEnemySpawners()
        {
            _configs = _dataService.ForLevelAndMode(_progress.Progress.WorldData.LevelId,
                _progress.Progress.WorldData.ModeId);

            List<SpawnPointConfig> enemySpawnPoints = _configs.Where(x => x.ActorType != EPlayerType.Player).ToList();
            CreateSpawners(enemySpawnPoints);

            List<SpawnPointConfig> playerPoints = _configs.Where(x => x.ActorType != EPlayerType.AI).ToList();
            CreatePlayers(playerPoints);
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