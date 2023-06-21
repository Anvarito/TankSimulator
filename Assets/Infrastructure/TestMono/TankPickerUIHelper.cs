
using System;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Infrastructure.Services.StaticData;
using System.Collections.Generic;
using Infrastructure.Services.StaticData.Tank;
using TMPro;
public class TankPickerUIHelper : UIHelper
{
    [SerializeField] private PlayerTankPickVeiw _playerTankPickVeiw;
    [SerializeField] private InputSystemUIInputModule _inputModule;

    private Infrastructure.Services.Input.PlayerConfiguration _playerConfiguration;
    private IStaticDataService _staticDataService;
    private List<TankId> _tanksId = new List<TankId>();
    private int _choiseIndex = 0;

    [HideInInspector] public UnityEvent OnTankChoise;

    public void Start()
    {
        base.Start();
        AddListeners();
    }

    public void OnDestroy()
    {
        _tanksId.Clear();
        RemoveListeners();
    }

    public void Construct(Infrastructure.Services.Input.PlayerConfiguration playerConfiguration, IStaticDataService staticDataService)
    {
        _staticDataService = staticDataService;
        _playerConfiguration = playerConfiguration;

        foreach (var tankId in staticDataService.Tanks)
        {
            _tanksId.Add(tankId.Key);
        }

        _playerTankPickVeiw.Initing( playerConfiguration.PlayerIndex);
        _playerTankPickVeiw.ShowTank(_staticDataService.ForTank(_tanksId[_choiseIndex]));
    }

    

    private void AddListeners()
    {
        _inputModule.move.action.performed += Move;
        _inputModule.submit.action.performed += Submit;
        _inputModule.scrollWheel.action.performed += ScrollMove;
        _inputModule.scrollWheel.action.canceled += ScrollStop;
    }

    private void ScrollStop(InputAction.CallbackContext obj)
    {
        _playerTankPickVeiw.Rotate(0);
    }

    private void ScrollMove(InputAction.CallbackContext input)
    {
        _playerTankPickVeiw.Rotate(input.ReadValue<Vector2>().x);
    }

    private void Submit(InputAction.CallbackContext input)
    {
        RemoveListeners();

        _playerConfiguration.IsReady = true;
        _playerConfiguration.PrefabPath = _staticDataService.ForTank(_tanksId[_choiseIndex]).PrefabPath;

        _playerTankPickVeiw.Submit();

        OnTankChoise?.Invoke();
    }

    private void Move(InputAction.CallbackContext input)
    {
        if (input.ReadValue<Vector2>().x == -1)
        {
            _choiseIndex = ClampIndex(--_choiseIndex);
            _playerTankPickVeiw.ShowTank(_staticDataService.ForTank(_tanksId[_choiseIndex]));
        }
        else if (input.ReadValue<Vector2>().x == 1)
        {
            _choiseIndex = ClampIndex(++_choiseIndex);
            _playerTankPickVeiw.ShowTank(_staticDataService.ForTank(_tanksId[_choiseIndex]));
        }
    }

    

    private int ClampIndex(int index)
    {
        if (index > _tanksId.Count - 1)
            index = 0;
        if (index < 0)
            index = _tanksId.Count - 1;

        return index;
    }

    private void RemoveListeners()
    {
        _inputModule.move.action.performed -= Move;
        _inputModule.submit.action.performed -= Submit;
        _inputModule.scrollWheel.action.performed -= ScrollMove;
        _inputModule.scrollWheel.action.canceled -= ScrollStop;
    }


}