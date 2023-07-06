using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        void ConnectToInputs(GameObject uiInputModule, bool individually = false);
        Action OnPlayerJoined { get; set; }
        List<PlayerConfiguration> PlayerConfigs { get; }
        Action OnEscTriggered { get; set; }
        void ResetPlayerIndex();

        void ResetToDefault();
    }
}