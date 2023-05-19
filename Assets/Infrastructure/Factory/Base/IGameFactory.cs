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
        PlayerUiParts PlayerParts { get; }
        GameOverBoard GameBoard { get; }

        GameObject CreatePlayer(GameObject at);
        void CreateTankUiSpawner(PlayerUiParts gameFactoryPlayerUiParts);
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
        GameObject CreatePlayerInputManager();
        PlayerInputManager PlayerInputManager { get; }
        void AngarCanvas();
        void CretePleasePressButtonPanel();
    }
}