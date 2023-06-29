using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Input;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Score;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.Timer;
using Infrastructure.TestMono;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory
{
    public class PlayerFactory : GameFactory, IPlayerFactory
    {
        public Action OnPlayerDestroyed { get; set; }
        public int PlayerCount => PlayerParts.Count;
        public List<PlayerUiParts> PlayerParts { get; } = new List<PlayerUiParts>();
        public MainMenuUIHelper MainMenuUIHelper { get; private set; }
        public GameOverBoard GameBoard { get; private set; }

        private readonly IInputService _inputService;
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _dataService;
        private readonly ITimerService _timer;
        private readonly IScoreCounter _scoreCounter;


        private readonly List<GameObject> _players = new List<GameObject>();
        private readonly List<ID_Settings_CS> _enemysID = new List<ID_Settings_CS>();


        public PlayerFactory(IAssetLoader assetLoader, IInputService inputService, IProgressService progressService, IStaticDataService dataService, ITimerService timer, IScoreCounter scoreCounter) : base(assetLoader)
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

            foreach (var i in _players)
            {
                Object.Destroy(i.gameObject);
            }

            PlayerParts.Clear();

            _enemysID.Clear();
        }

        public void CreatePlayers(List<SpawnPointConfig> points)
        {
            
            //at = Shuffle(at);
            Dictionary<ID_Settings_CS, int> _indexById = new Dictionary<ID_Settings_CS, int>();

            foreach (PlayerConfiguration config in _inputService.PlayerConfigs)
            {
                var filteredPoints = points.Where(x => x.Team == config.Team);
                SpawnPointConfig selectedPoint = filteredPoints.First();
                points.Remove(selectedPoint);
                
                GameObject player = InstantiateRegistered(config.PrefabPath,
                    selectedPoint.Position);
                _players.Add(player);
                PlayerUiParts registerUiWatchers = RegisterUiWatchers(player);
                registerUiWatchers.DamageReceiver.OnTankDestroyed.AddListener(PlayerDestroyed);

                PlayerParts.Add(registerUiWatchers);

                registerUiWatchers.IdSettings.SetRelationship(ERelationship.TeamA);
                InitedRegisteredTank(player, config);
                
                _scoreCounter.AddPlayerIndex(registerUiWatchers.IdSettings, config.PlayerIndex);
            }

        }



        public void CreateTankUiSpawners(List<DamageReceiversManager> enemyDamageManagers)
        {
            foreach (DamageReceiversManager damageReceivers in enemyDamageManagers)
            {
                _enemysID.Add(damageReceivers.GetComponentInParent<ID_Settings_CS>());
            }

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
            GamemodeMapHelper helper = InstantiateRegistered(AssetPaths.MapModeMenu).GetComponentInChildren<GamemodeMapHelper>();
            helper.Construct(_progressService, _dataService, _inputService);
            return helper;
        }

        private void PlayerDestroyed(ID_Settings_CS killerId) =>
            OnPlayerDestroyed?.Invoke();


        private void InitedRegisteredTank(GameObject playerTank, Services.Input.PlayerConfiguration config)
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
            RecivierUIManager recivierUIManager = uiSpawner.GetComponent<RecivierUIManager>();
            recivierUIManager.Initialize(parts.Aiming, parts.BulletGenerator, parts.CannonFire, parts.GunCamera, parts.DamageReceiver, parts.DriveControl, parts.CameraView, _enemysID, parts.IdSettings, _timer, _scoreCounter);
        }

        //private GameObject[] Shuffle(GameObject[] at)
        //{
        //    var random = new Random();
        //    for (int i = at.Length - 1; i >= 1; i--)
        //    {
        //        int j = random.Next(i + 1);

        //        (at[j], at[i]) = (at[i], at[j]);
        //    }

        //    return at;
        //}
    }
}