using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factory
{
    public class InputFactory : GameFactory, IInputFactory
    {
        public PlayerInputManager PlayerInputManager { get; }

        public InputFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public GameObject CreatePlayerInputManager() =>
            _assetLoader.Instantiate(AssetPaths.PlayerInputManager);


        public void AngarCanvas()
        {
            Debug.Log("Spawned canvas");
        }

        public void CretePleasePressButtonPanel() =>
            _assetLoader.Instantiate(AssetPaths.PlsPressButtonPanel);

        public Transform CreatePickerCanvas() =>
            _assetLoader.Instantiate(AssetPaths.TankPickerCanvas).transform;

        public GameObject CreateTankPickerUI(Transform parent)
        {
            GameObject tankPicker = _assetLoader.Instantiate(AssetPaths.PlayerInMenu);
            tankPicker.transform.SetParent(parent);
            return tankPicker;
        }
    }
}