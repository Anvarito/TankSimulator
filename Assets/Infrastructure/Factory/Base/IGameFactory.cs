using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Services.Progress;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factory.Base
{
    public interface IGameFactory : IFactory
    {
        List<IProgressWriter> ProgressWriters { get; }
        List<IProgressReader> ProgressReaders { get; }
        void CleanUp();
    }

    public interface IPlayerFactory : IGameFactory
    {
        List<PlayerUiParts> PlayerParts { get; }
        GameOverBoard GameBoard { get; }

        void CreatePlayers(GameObject[] at);
        void CreateHud();


        MainMenuUIHelper MainMenuUIHelper { get; }
        GameObject CreateMainMenu();
    }

    public interface IEnemyFactory : IGameFactory
    {
        List<DamageReciviersManager> EnemyDamageManagers { get; }
        void CreateEnemies(GameObject[] at);
        public void CreateTankController();
    }
    
    internal interface IInputFactory : IGameFactory
    {
        List<TankPickerUIHelper> TankPickerUIHelpers { get; }
        PlayerInputManager PlayerInputManager { get; }
        GameObject CreatePlayerInputManager();
        void CretePleasePressButtonPanel();
        GameObject CreateTankPickerUI(Transform parent);
        Transform CreatePickerCanvas();
    }
}