using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IFactory
    {
        GameObject CreatePlayer(GameObject at);
        void CreateHud();
        void CreateEnemies(GameObject[] at);
    }
}