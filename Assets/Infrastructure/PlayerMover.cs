using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
    public class PlayerMover : MonoBehaviour
    {
        private IInputService _input;

        private void Awake()
        {
            _input = ServiceLocator.Container.Single<IInputService>();
        }
    }
}
