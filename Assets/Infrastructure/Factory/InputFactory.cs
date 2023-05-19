using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factory
{
    public class InputFactory : GameFactory, IInputFactory
    {
        public InputFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public GameObject CreatePlayerInputManager() => 
            _assetLoader.Instantiate(AssetPaths.PlayerInputManager);
        
        
        public PlayerInputManager PlayerInputManager { get; }

        public void AngarCanvas()
        {
            Debug.Log("Spawned canvas");
        }

        public void CretePleasePressButtonPanel() => 
            _assetLoader.Instantiate(AssetPaths.PlsPressButtonPanel);
    }
}