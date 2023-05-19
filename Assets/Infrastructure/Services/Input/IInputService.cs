using System;
using UnityEngine.InputSystem.UI;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        void ConnectToInputs(InputSystemUIInputModule uiInputModule);
        Action OnPlayerJoined { get; set; }
    }
}