using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using UnityEngine.InputSystem;

namespace Infrastructure.StateMachine
{
    public class IntroVideoState : IState
    {
        private const string _videoSceneName = "VideoIntroScene";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private IAudioService _audioService;

        public IntroVideoState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IInputService inputServise, IAudioService audioService)
        {
            _audioService = audioService;
            _sceneLoader = sceneLoader;
            _inputService = inputServise;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            //_audioService.ChangeMusicVolume(0);
            _audioService.StopSound();
            _audioService.StopMusic();
            _sceneLoader.Load(_videoSceneName, OnLoad);
        }
        private void OnLoad()
        {
            _inputService.PlayerConfigs.First().Input.onActionTriggered += OnAbort;
        }
        private void OnAbort(InputAction.CallbackContext obj)
        {
            if (obj.performed && obj.action.name == "Fire")
            {
                _gameStateMachine.Enter<MenuState>();
            }
        }

        public void Exit()
        {
            _inputService.PlayerConfigs.First().Input.onActionTriggered -= OnAbort;
        }
    }
}
