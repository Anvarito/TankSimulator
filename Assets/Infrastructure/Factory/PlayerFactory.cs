using System;
using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Input;
using Infrastructure.TestMono;
using UnityEngine;
using Random = System.Random;

namespace Infrastructure.Factory
{
    public class PlayerFactory : GameFactory, IPlayerFactory
    {
        private readonly IInputService _inputService;
        public List<PlayerUiParts> PlayerParts { get; } = new List<PlayerUiParts>();
        public MainMenuUIHelper MainMenuUIHelper { get; private set; }
        public GameOverBoard GameBoard => _gameOverBoard;

        private GameOverBoard _gameOverBoard;

        private List<ID_Settings_CS> _enemysID = new List<ID_Settings_CS>();

        public PlayerFactory(IAssetLoader assetLoader, IInputService inputService) : base(assetLoader)
        {
            _inputService = inputService;

        }

        public void CreatePlayers(Vector3 at)
        {
            //at = Shuffle(at);
            foreach (Services.Input.PlayerConfiguration config in _inputService.PlayerConfigs)
            {
                GameObject player = InstantiateRegistered(AssetPaths.PlayerTank, at);
                PlayerParts.Add(RegisterUiWatchers(player));
                InitedRegisteredTank(player, config);
            }
        }

        public void CreateTankUiSpawners(List<DamageReciviersManager> enemyDamageManagers)
        {
            for (int i = 0; i < enemyDamageManagers.Count; i++)
            {
                _enemysID.Add(enemyDamageManagers[i].GetComponentInParent<ID_Settings_CS>());
            }

            foreach (PlayerUiParts part in PlayerParts)
                InitializeUiWatchers(part, InstantiateRegistered(AssetPaths.TankUiSpawner));
        }

        public void CreateHud() =>
            _gameOverBoard = InstantiateRegistered(AssetPaths.Hud).GetComponentInChildren<GameOverBoard>();

        public GameObject CreateMainMenu() =>
            RegisterMainMenuUIHelper(InstantiateRegistered(AssetPaths.MainMenu));


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
            player.DamageReceiver = gameObject.GetComponentInChildren<DamageReciviersManager>();
            player.DriveControl = gameObject.GetComponentInChildren<Drive_Control_CS>();
            player.CameraView = gameObject.GetComponentInChildren<CameraViewSetup>();
            player.IdSettings = gameObject.GetComponentInChildren<ID_Settings_CS>();
            return player;
        }

        private void InitializeUiWatchers(PlayerUiParts parts, GameObject uiSpawner)
        {
            RecivierUIManager recivierUIManager = uiSpawner.GetComponent<RecivierUIManager>();
            recivierUIManager.Initialize(parts.Aiming, parts.BulletGenerator, parts.CannonFire, parts.GunCamera, parts.DamageReceiver, parts.DriveControl, parts.CameraView, _enemysID, parts.IdSettings);
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