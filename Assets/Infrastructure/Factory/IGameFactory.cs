using System.Collections.Generic;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IFactory
    {
        GameObject CreatePlayer(GameObject at);
        void CreateHud();
        void CreateEnemies(GameObject[] at);
        List<IProgressReader> ProgressReaders { get; }
        List<IProgressWriter> ProgressWriters { get; }
        void CleanUp();
    }
}