using System.Collections.Generic;
using Infrastructure.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factory.Base
{
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