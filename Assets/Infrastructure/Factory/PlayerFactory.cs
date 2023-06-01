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
                PlayerParts.Add(InstantiateRegisterTank(at[config.PlayerIndex]));
        }

        public void CreateTankUiSpawners()
        {
            foreach (PlayerUiParts part in PlayerParts)
                InitializeUiWatchers(part, InstantiateRegistered(AssetPaths.TankUiSpawner));
        }

        public void CreateHud() =>
            _gameOverBoard = InstantiateRegistered(AssetPaths.Hud).GetComponentInChildren<GameOverBoard>();

        public GameObject CreateMainMenu() =>
            RegisterMainMenuUIHelper(InstantiateRegistered(AssetPaths.MainMenu));


        private PlayerUiParts InstantiateRegisterTank(GameObject at) =>
            RegisterUiWatchers(InstantiateRegistered(AssetPaths.PlayerTank, at.transform.position));

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

        private void InitializeUiWatchers(PlayerUiParts parts, GameObject uiSpawner)
        {
            uiSpawner.GetComponent<AimMarkerUIReceiver>().Initialize(parts.Aiming);
            uiSpawner.GetComponent<LeadMarkerUIReceiver>().Initialize(parts.Aiming, parts.BulletGenerator);
            uiSpawner.GetComponent<ReloadingCircleUIReseiver>().Initialize(parts.CannonFire);
            uiSpawner.GetComponent<ReticleControlUIReceiver>().Initialize(parts.GunCamera);
            uiSpawner.GetComponent<HitPoitsBarUIReceiver>().Initialize(parts.DamageReceiver);
            uiSpawner.GetComponent<SpeedIndicatorRecivier>().Initialize(parts.DriveControl);
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