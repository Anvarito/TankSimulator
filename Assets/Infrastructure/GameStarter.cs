using UnityEngine;

namespace Infrastructure
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _gameBootstrapper;

        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();
            if (bootstrapper == null)
                Instantiate(_gameBootstrapper);
        }
    }
}