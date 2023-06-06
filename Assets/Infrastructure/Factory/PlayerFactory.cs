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


        public PlayerFactory(IAssetLoader assetLoader, IInputService inputService) : base(assetLoader)
        {
            _inputService = inputService;
        }

        public void CreatePlayers(GameObject[] at)
        {
            at = Shuffle(at);
            foreach (Services.Input.PlayerConfiguration config in _inputService.PlayerConfigs)
                PlayerParts.Add(InstantiateRegisterTank(at[config.PlayerIndex], config));
        }

        public void CreateHud() =>
            _gameOverBoard = InstantiateRegistered(AssetPaths.Hud).GetComponentInChildren<GameOverBoard>();

        public GameObject CreateMainMenu() =>
            RegisterMainMenuUIHelper(InstantiateRegistered(AssetPaths.MainMenu));


        private PlayerUiParts InstantiateRegisterTank(GameObject at, Services.Input.PlayerConfiguration config)
        {
            GameObject tank = InstantiateRegistered(AssetPaths.PlayerNewTank, at.transform.position);
            SetupTank(tank,config);
            return RegisterUiWatchers(tank);
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
            return player;
        }

        private void SetupTank(GameObject tank, Services.Input.PlayerConfiguration config)
        {
            PlayerInputInitializer player = tank.GetComponentInChildren<PlayerInputInitializer>(); 
            
            player.GetComponent<CameraViewSetup>().SetupLayoutScreen(config.PlayerIndex, PlayerParts.Count);
            player.GetComponent<CameraViewSetup>().SetScreenAimPoint(config.PlayerIndex, PlayerParts.Count);

            player.SetPlayerInput(config.Input);
            player.Initialize();

            player.GetComponentInChildren<RecivierUIManager>().Initialize();
        }

        private GameObject[] Shuffle(GameObject[] at)
        {
            var random = new Random();
            for (int i = at.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);

                (at[j], at[i]) = (at[i], at[j]);
            }

            return at;
        }
    }
}