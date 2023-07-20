using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Tank;

namespace Infrastructure.StateMachine
{
    class LaunchTrainingLevel : IState
    {
        private GameStateMachine _gameStateMachine;
        private IInputService _inputServise;
        private IProgressService _progressService;
        private IStaticDataService _staticDataService;

        private const string TrainLevel = "Poligon";

        public LaunchTrainingLevel(GameStateMachine gameStateMachine, IInputService inputService, IStaticDataService staticDataService, IProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _inputServise = inputService;
        }

        public void Enter()
        {
            _progressService.Progress.WorldData.ModeId = Services.StaticData.Gamemodes.GamemodeId.Training;
            _progressService.Progress.WorldData.LevelId = Services.StaticData.Level.LevelId.Training;
            _progressService.Progress.WorldData.Level = TrainLevel;

            var playerConfiguration = _inputServise.PlayerConfigs.First();
            playerConfiguration.IsReady = true;
            playerConfiguration.PrefabPath = _staticDataService.ForTank(TankId.Nona2S9).PrefabPath;

            _gameStateMachine.Enter<LoadLevelState, string>(TrainLevel);
        }


        public void Exit()
        {

        }
    }
}
