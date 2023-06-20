using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.TestMono;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerInitialPoint = "PlayerInitialPoint";
        private const string EnemyInitialPoint = "EnemyInitialPoint";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;
        private readonly IFactories _factories;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;
        // private readonly IProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,IProgressService progress, IStaticDataService dataService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progress = progress;
            _dataService = dataService;
            _factories = factories;

            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }

        public void Enter(string payload)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _playerFactory.CleanUp();
            _enemyFactory.CleanUp();
            
            _sceneLoader.Load(name: payload, OnLoaded);
        }

        private void OnLoaded()
        {
            InitGameLevel();
            // InformProgressReaders();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        // private void InformProgressReaders()
        // {
        //     foreach (IProgressReader progress in _playerFactory.ProgressReaders) 
        //         progress.LoadProgress(_progressService.Progress);
        //     
        //     foreach (IProgressReader progress in _enemyFactory.ProgressReaders) 
        //         progress.LoadProgress(_progressService.Progress);
        // }

        private void InitGameLevel()
        {
            //TeamSeparator teamSeparator = Object.FindObjectOfType<TeamSeparator>();
            CreateSpawners();


            _enemyFactory.CreateGameController();
            //
            // CreatePlayers(teamSeparator);
        }

        private void CreateSpawners()
        {
            List<SpawnPointConfig> configs = _dataService.ForLevelAndMode(_progress.Progress.WorldData.LevelId, _progress.Progress.WorldData.ModeId);

            GameObject parent = new GameObject("Spawners");
            
            foreach (SpawnPointConfig enemyConfig in configs.Where(x => x.ActorType != EPlayerType.Player))
            {
                GameSpawnPoint point = new GameObject("SpawnPoint").AddComponent<GameSpawnPoint>();
                point.transform.SetParent(parent.transform);
                point.Construct(_factories, enemyConfig);
            }

            var playerPoints = configs.Where(x => x.ActorType != EPlayerType.AI && x.Team == ERelationship.TeamA).ToList();
            CreatePlayers(playerPoints);
        }

        private void CreatePlayers(List<SpawnPointConfig> spawnPointConfigs)
        {
            _playerFactory.CreatePlayers(spawnPointConfigs);
            _playerFactory.CreateTankUiSpawners(_enemyFactory.EnemyDamageManagers);
            _playerFactory.CreateHud();
        }
    }
}