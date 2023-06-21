using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Tank;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankPickVeiw : MonoBehaviour
{
    [SerializeField] private GameObject _namePanel;
    [SerializeField] private TextMeshProUGUI _readyText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TankPickerUIHelper _tankPickerUIHelper;

    [Space(10)]
    [Header("Colors")]
    [SerializeField] private Color _notReadyColor;
    [SerializeField] private Color _readyColor;

    private GameObject _choiseTank;
    private Transform _spawnPoint;
    private float _rotateAmount = 0;

    public void Initing(int playerIndex)
    {
        SetNotReady();

        _spawnPoint = playerIndex == 0 ? GameObject.Find("Player1DemoPoint").transform : GameObject.Find("Player2DemoPoint").transform;

        if (playerIndex == 1)
            Destroy(GameObject.Find("Player2ConnectHelp"));

        SetUIposition();
    }
    private void SetUIposition()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(_spawnPoint.position);
        transform.position = new Vector3(pos.x, Screen.height / 2, 0);
        transform.localScale = Vector3.one;
    }
    public void ShowTank(TankConfig tank)
    {
        if (_choiseTank != null)
            Destroy(_choiseTank.gameObject);

        _choiseTank = Instantiate(tank.PrefabEmpty, _spawnPoint.position, _spawnPoint.rotation);
        _nameText.text = tank.Name;
    }

    internal void Submit()
    {
        SetReady();
        _namePanel.SetActive(false);
    }


    public void Rotate(float amount)
    {
        _rotateAmount = amount;
    }
    private void Update()
    {
        if (_choiseTank == null)
            return;

        _choiseTank.transform.Rotate(0, -_rotateAmount * 0.4f, 0);
    }

    private void SetReady()
    {
        _readyText.text = "Игрок готов!";
        _readyText.color = _readyColor;
    }

    private void SetNotReady()
    {
        _readyText.text = "Выберите танк";
        _readyText.color = _notReadyColor;
    }

}
