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
    [SerializeField] private TextMeshProUGUI _readyText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private GameObject _namePanel;
    [SerializeField] private InputSystemUIInputModule _inputModule;
    private Infrastructure.Services.Input.PlayerConfiguration _playerConfiguration;

    [HideInInspector] public UnityEvent OnTankChoise;
    private IStaticDataService _staticDataService;
    private List<TankId> _tanksId = new List<TankId>();

    private int _choiseIndex = 0;
    private GameObject _choiseTank;
    private Transform _spawnPoint;
    private float _rotateAmount = 0;

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

    public void Construct(Infrastructure.Services.Input.PlayerConfiguration playerConfiguration, IStaticDataService staticDataService)
    {
        _staticDataService = staticDataService;
        _playerConfiguration = playerConfiguration;

        foreach (var tankId in staticDataService.Tanks)
        {
            _tanksId.Add(tankId.Key);
        }

        _spawnPoint = playerConfiguration.PlayerIndex == 0 ? GameObject.Find("Player1DemoPoint").transform : GameObject.Find("Player2DemoPoint").transform;

        if (playerConfiguration.PlayerIndex == 1)
            Destroy(GameObject.Find("Player2ConnectHelp"));

        ShowTank();

        SetUIposition();
    }

    private void SetUIposition()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(_choiseTank.transform.position);
        transform.position = new Vector3(pos.x, Screen.height / 2, 0);
    }

    private void AddListeners()
    {
        _inputModule.move.action.performed += Move;
        _inputModule.submit.action.performed += Submit;
        _inputModule.scrollWheel.action.performed += Rotate;
        _inputModule.scrollWheel.action.canceled += StopRotate;
    }

    private void StopRotate(InputAction.CallbackContext obj)
    {
        _rotateAmount = 0;
    }

    private void Rotate(InputAction.CallbackContext input)
    {
        _rotateAmount = input.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        if (_choiseTank == null)
            return;

        _choiseTank.transform.Rotate(0, -_rotateAmount * 0.4f ,0);
    }

    private void Submit(InputAction.CallbackContext input)
    {
        SetReady();
        RemoveListeners();

        _namePanel.SetActive(false);

        _playerConfiguration.IsReady = true;
        _playerConfiguration.PrefabPath = _staticDataService.ForTank(_tanksId[_choiseIndex]).PrefabPath;

        OnTankChoise?.Invoke();
    }

    private void Move(InputAction.CallbackContext input)
    {
        if (input.ReadValue<Vector2>().x == -1)
        {
            _choiseIndex = ClampIndex(--_choiseIndex);
            ShowTank();
            OnRight?.Invoke();
        }
        else if (input.ReadValue<Vector2>().x == 1)
        {
            _choiseIndex = ClampIndex(++_choiseIndex);
            ShowTank();
            OnLeft?.Invoke();
        }
    }

    private void ShowTank()
    {
        if (_choiseTank != null)
            Destroy(_choiseTank.gameObject);

        var tankCOnfig = _staticDataService.ForTank(_tanksId[_choiseIndex]);
        _choiseTank = Instantiate(tankCOnfig.PrefabEmpty, _spawnPoint.position, _spawnPoint.rotation);
        _nameText.text = tankCOnfig.Name;
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
    }

    private void SetReady()
    {
        _readyText.text = "Игрок готов!";
        _readyText.color = Color.green;
    }

    private void SetNotReady()
    {
        _readyText.text = "Выберите танк";
        _readyText.color = Color.red;
    }


}