using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerConfigHolder : MonoBehaviour
{
    [SerializeField] private PlayerInMenu playerSetupMenuPrefab;
    [SerializeField] private PlayerInput input;

    public void Initialized(Canvas mainCanvas)
    {
        PlayerInMenu playerMenu = Instantiate(playerSetupMenuPrefab, mainCanvas.transform);
        input.uiInputModule = playerMenu.GetInputSystemUIInput;
        playerMenu.SetPlayerIndex(input.playerIndex);
    }
}
