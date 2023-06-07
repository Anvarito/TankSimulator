using System;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class TankPickerUIHelper : UIHelper
{
    //[SerializeField] Button _firstTankButton;
    //[SerializeField] Button _secondTankButton;
    [SerializeField] TMPro.TextMeshProUGUI _readyText;
    [SerializeField] private InputSystemUIInputModule _inputModule;
    //public Action<Infrastructure.Services.Input.PlayerConfiguration> OnSecondTank;
    private Infrastructure.Services.Input.PlayerConfiguration _playerConfiguration;

    [HideInInspector] public UnityEvent<Infrastructure.Services.Input.PlayerConfiguration> OnTankChoise;
    //private UnityEvent OnSecondTankButton => _secondTankButton.onClick;

    [HideInInspector] public UnityEvent OnLeft;
    [HideInInspector] public UnityEvent OnRight;

    public void Start()
    {
        base.Start();
        SetNotReady();
        
        AddListeners();
    }

    public void OnDestroy() => 
        RemoveListeners();

    public void Construct(Infrastructure.Services.Input.PlayerConfiguration playerConfiguration) => 
        _playerConfiguration = playerConfiguration;

    private void AddListeners()
    {
        _inputModule.move.action.performed += Move;
        _inputModule.submit.action.performed += Submit;
        //OnFirstTankButton.AddListener(SetReady);
        //OnSecondTankButton.AddListener(SetReady);
        //OnFirstTankButton.AddListener(FirstClick);
        //OnSecondTankButton.AddListener(SecondClick);
    }

    private void Submit(InputAction.CallbackContext input)
    {
        SetReady();
        RemoveListeners();

        //_playerConfiguration.IsReady = true;
        //_playerConfiguration.TankIndex = 0;

        OnTankChoise?.Invoke(_playerConfiguration);
    }

    private void Move(InputAction.CallbackContext input)
    {
        if (input.ReadValue<Vector2>().x == -1)
        {
            OnRight?.Invoke();
        }
        else if (input.ReadValue<Vector2>().x == 1)
        {
            OnLeft?.Invoke();
        }
        
    }

    private void RemoveListeners()
    {
        //OnFirstTankButton.RemoveListener(SetReady);
        //OnSecondTankButton.RemoveListener(SetReady);
        //OnFirstTankButton.RemoveListener(FirstClick);
        //OnSecondTankButton.RemoveListener(SecondClick);
        _inputModule.move.action.performed -= Move;
        _inputModule.submit.action.performed -= Submit;
    }

    //private void FirstClick() => 
    //    OnFirstTank?.Invoke(_playerConfiguration);
    //private void SecondClick() => 
    //    OnSecondTank?.Invoke(_playerConfiguration);

    private void SetReady()
    {
        _readyText.text = "Player ready!";
        _readyText.color = Color.green;
    }

    private void SetNotReady()
    {
        _readyText.text = "Player not ready";
        _readyText.color = Color.red;
    }
    
    
}