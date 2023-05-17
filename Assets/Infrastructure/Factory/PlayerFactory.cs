using ChobiAssets.PTM;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.StateMachine;
using Infrastructure.TestMono;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class PlayerFactory : GameFactory, IPlayerFactory
    {
        public PlayerUiParts PlayerParts { get; } = new PlayerUiParts();
        public MainMenuUIHelper MainMenuUIHelper { get; private set; }
        public GameOverBoard GameBoard => _gameOverBoard;

        private GameOverBoard _gameOverBoard;


        public PlayerFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public GameObject CreatePlayer(GameObject at) =>
            InstantiateRegisterTank(at);

        public void CreateTankUiSpawner(PlayerUiParts parts) =>
            InitializeUiWatchers(parts, uiSpawner: InstantiateRegistered(AssetPaths.TankUiSpawner));

        public void CreateHud() =>
            _gameOverBoard = InstantiateRegistered(AssetPaths.Hud).GetComponentInChildren<GameOverBoard>();

        public GameObject CreateMainMenu() => 
            RegisterMainMenuUIHelper(InstantiateRegistered(AssetPaths.MainMenu));


        private GameObject InstantiateRegisterTank(GameObject at) =>
            RegisterUiWatchers(InstantiateRegistered(AssetPaths.PlayerTank, at.transform.position));

        private GameObject RegisterMainMenuUIHelper(GameObject mainMenu)
        {
            MainMenuUIHelper = mainMenu.GetComponentInChildren<MainMenuUIHelper>();
            return mainMenu;
        }

        private GameObject RegisterUiWatchers(GameObject gameObject)
        {
            PlayerParts.Aiming = gameObject.GetComponentInChildren<Aiming_Control_CS>();
            PlayerParts.BulletGenerator = gameObject.GetComponentInChildren<Bullet_Generator_CS>();
            PlayerParts.CannonFire = gameObject.GetComponentInChildren<Cannon_Fire_CS>();
            PlayerParts.GunCamera = gameObject.GetComponentInChildren<Gun_Camera_CS>();
            PlayerParts.DamageReceiver = gameObject.GetComponentInChildren<DamageReciviersManager>();
            PlayerParts.DriveControl = gameObject.GetComponentInChildren<Drive_Control_CS>();
            return gameObject;
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
    }
}