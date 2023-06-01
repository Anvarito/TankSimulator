using System;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TankPickerUIHelper : UIHelper
{
    [SerializeField] Button _firstTankButton;
    [SerializeField] Button _secondTankButton;
    [SerializeField] TMPro.TextMeshProUGUI _readyText;

    public Action<Infrastructure.Services.Input.PlayerConfiguration> OnFirstTank;
    public Action<Infrastructure.Services.Input.PlayerConfiguration> OnSecondTank;
    private Infrastructure.Services.Input.PlayerConfiguration _playerConfiguration;

    private UnityEvent OnFirstTankButton => _firstTankButton.onClick;
    private UnityEvent OnSecondTankButton => _secondTankButton.onClick;

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
        OnFirstTankButton.AddListener(SetReady);
        OnSecondTankButton.AddListener(SetReady);
        OnFirstTankButton.AddListener(FirstClick);
        OnSecondTankButton.AddListener(SecondClick);
    }

    private void RemoveListeners()
    {
        OnFirstTankButton.RemoveListener(SetReady);
        OnSecondTankButton.RemoveListener(SetReady);
        OnFirstTankButton.RemoveListener(FirstClick);
        OnSecondTankButton.RemoveListener(SecondClick);
    }

    private void FirstClick() => 
        OnFirstTank?.Invoke(_playerConfiguration);
    private void SecondClick() => 
        OnSecondTank?.Invoke(_playerConfiguration);

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