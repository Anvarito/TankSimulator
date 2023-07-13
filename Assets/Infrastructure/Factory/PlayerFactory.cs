using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Components;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Score;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.Timer;
using Infrastructure.TestMono;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class PlayerFactory : GameFactory, IPlayerFactory
    {
        public event Action<ID_Settings_CS, ID_Settings_CS> OnPlayerDestroyed;
        public int PlayerCount => PlayerParts.Count;
        public List<PlayerUiParts> PlayerParts { get; } = new();
        public List<ID_Settings_CS> PlayersSettings { get; } = new();
        public List<ID_Settings_CS> EnemysID { get; } = new();
        public MainMenuUIHelper MainMenuUIHelper { get; private set; }
        public GameOverBoard GameBoard { get; private set; }


        private readonly IInputService _inputService;
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _dataService;
        private readonly ITimerService _timer;
        private readonly IScoreCounter _scoreCounter;


        public PlayerFactory(IAudioService audioService, IAssetLoader assetLoader, IInputService inputService,
            IProgressService progressService, IStaticDataService dataService, ITimerService timer,
            IScoreCounter scoreCounter) : base(audioService, assetLoader)
        {
            _inputService = inputService;
            _progressService = progressService;
            _dataService = dataService;
            _timer = timer;
            _scoreCounter = scoreCounter;
        }

        public override void CleanUp()
        {
            base.CleanUp();

            foreach (var i in PlayersSettings)
            {
                // Object.Destroy(i.gameObject);
            }

            PlayersSettings.Clear();

            PlayerParts.Clear();

            EnemysID.Clear();
        }

        public void CreatePlayers(List<SpawnPointConfig> points)
        {
            foreach (PlayerConfiguration config in _inputService.PlayerConfigs)
            {
                var filteredPoints = points.Where(x => x.Team == config.Team);
                SpawnPointConfig selectedPoint = filteredPoints.First();
                points.Remove(selectedPoint);

                GameObject player = InstantiateRegistered(config.PrefabPath, selectedPoint.Position);
                PlayersSettings.Add(player.GetComponentInChildren<ID_Settings_CS>());
                PlayerUiParts registerUiWatchers = RegisterUiWatchers(player);
                registerUiWatchers.DamageReceiver.OnTankDestroyed.AddListener(PlayerDestroyed);

                PlayerParts.Add(registerUiWatchers);
                registerUiWatchers.IdSettings.SetRelationship(selectedPoint.Team);
                InitedRegisteredTank(player, config);

                _scoreCounter.AddPlayerIndex(registerUiWatchers.IdSettings, config.PlayerIndex);
            }
        }


        public void CreateTankUiSpawners(List<DamageReceiversManager> enemyDamageManagers)
        {
            EnemysID.AddRange(PlayerParts.Select(x => x.IdSettings));
            EnemysID.AddRange(enemyDamageManagers.Select(x => x.GetComponentInParent<ID_Settings_CS>()));

            foreach (PlayerUiParts part in PlayerParts)
                InitializeUiWatchers(part, InstantiateRegistered(AssetPaths.TankUiSpawner));
        }

        public void CreateHud()
        {
            var hud = InstantiateRegistered(AssetPaths.Hud);
            GameBoard = hud.GetComponentInChildren<GameOverBoard>();
        }

        public GameObject CreateMainMenu() =>
            RegisterMainMenuUIHelper(InstantiateRegistered(AssetPaths.MainMenu));

        public GamemodeMapHelper CreateMapModeChoiseUI()
        {
            GamemodeMapHelper helper = InstantiateRegistered(AssetPaths.MapModeMenu)
                .GetComponentInChildren<GamemodeMapHelper>();
            helper.Construct(_progressService, _dataService, _inputService);
            return helper;
        }

        private void PlayerDestroyed(ID_Settings_CS playerId, ID_Settings_CS killerId)
        {
            PlayersSettings.Remove(playerId);
            OnPlayerDestroyed?.Invoke(playerId, killerId);
        }


        private void InitedRegisteredTank(GameObject playerTank, PlayerConfiguration config)
        {
            int playerCount = _inputService.PlayerConfigs.Count;
            playerTank.GetComponent<CameraViewSetup>().SetupLayoutScreen(config.PlayerIndex, playerCount);
            playerTank.GetComponent<CameraViewSetup>().SetScreenAimPoint(config.PlayerIndex, playerCount);

            playerTank.GetComponent<PlayerInputInitializer>().SetPlayerInput(config.Input);
            playerTank.GetComponent<PlayerInputInitializer>().Initialize();
        }

        private GameObject RegisterMainMenuUIHelper(GameObject mainMenu)
        {
            MainMenuUIHelper = mainMenu.GetComponentInChildren<MainMenuUIHelper>();
            return mainMenu;
        }

        private PlayerUiParts RegisterUiWatchers(GameObject gameObject)
        {
            var player = new PlayerUiParts();

            player.Aiming = gameObject.GetComponentInChildren<Aiming_Control_CS>();
            player.BulletGenerator = gameObject.GetComponentInChildren<Bullet_Generator_CS>();
            player.CannonFire = gameObject.GetComponentInChildren<Cannon_Fire_CS>();
            player.GunCamera = gameObject.GetComponentInChildren<Gun_Camera_CS>();
            player.DamageReceiver = gameObject.GetComponentInChildren<DamageReceiversManager>();
            player.DriveControl = gameObject.GetComponentInChildren<Drive_Control_CS>();
            player.CameraView = gameObject.GetComponentInChildren<CameraViewSetup>();
            player.IdSettings = gameObject.GetComponentInChildren<ID_Settings_CS>();

            return player;
        }

        private void InitializeUiWatchers(PlayerUiParts parts, GameObject uiSpawner)
        {
            parts.RecivierUIManager = uiSpawner.GetComponent<RecivierUIManager>();
            parts.RecivierUIManager.Initialize(parts.Aiming, parts.BulletGenerator, parts.CannonFire, parts.GunCamera,
                parts.DamageReceiver, parts.DriveControl, parts.CameraView, EnemysID, parts.IdSettings, _timer,
                _scoreCounter);
        }

        public void AddNewEnemyToPositionActorsUI(ID_Settings_CS newEnemy)
        {
            for (int i = 0; i < PlayerParts.Count; i++)
            {
                PlayerParts[i].RecivierUIManager.AddNewEnemyToPositionActorsUI(newEnemy);
            }
        }
    }
}