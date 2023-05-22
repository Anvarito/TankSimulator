using System;
using UnityEngine.InputSystem.UI;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        void ConnectToInputs(InputSystemUIInputModule uiInputModule, bool individually = false);
        Action OnPlayerJoined { get; set; }
        void ResetPlayerIndex();
    }
}