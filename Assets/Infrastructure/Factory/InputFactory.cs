using System.Collections.Generic;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factory
{
    public class InputFactory : GameFactory, IInputFactory
    {
        public PlayerInputManager PlayerInputManager { get; }
        public List<TankPickerUIHelper> TankPickerUIHelpers { get; private set; } = new List<TankPickerUIHelper>();

        public InputFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public GameObject CreatePlayerInputManager() =>
            _assetLoader.Instantiate(AssetPaths.PlayerInputManager);
        
        public void CretePleasePressButtonPanel() =>
            _assetLoader.Instantiate(AssetPaths.PlsPressButtonPanel);

        public Transform CreatePickerCanvas() =>
            _assetLoader.Instantiate(AssetPaths.TankPickerCanvas).transform;

        public GameObject CreateTankPickerUI(Transform parent)
        {
            GameObject tankPicker = _assetLoader.Instantiate(AssetPaths.PlayerInMenu);
            TankPickerUIHelpers.Add(tankPicker.GetComponentInChildren<TankPickerUIHelper>());
            tankPicker.transform.SetParent(parent);
            return tankPicker;
        }
    }
}