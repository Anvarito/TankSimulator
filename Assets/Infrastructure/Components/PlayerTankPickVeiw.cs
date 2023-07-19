using System.Collections;
using Infrastructure.Components;
using Infrastructure.Services.StaticData.Tank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankPickVeiw : MonoBehaviour
{
    [SerializeField] private GameObject _namePanel;
    [SerializeField] private TextMeshProUGUI _readyText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TankPickerUIHelper _tankPickerUIHelper;
    [SerializeField] private GameObject _hoistPrefab;
    [SerializeField] private TextMeshProUGUI _playerName;

    [Space(10)]
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI _tankInfo;
    [SerializeField] private RectTransform _content;

    [Space(10)]
    [Header("Colors")]
    [SerializeField] private Color _notReadyColor;
    [SerializeField] private Color _readyColor;

    private GameObject _choiseTank;
    private Transform _spawnPoint;
    private float _rotateAmount = 0;
    private float _scrollAmount;

    public void Initing(int playerIndex)
    {
        SetNotReady();

        _spawnPoint = playerIndex == 0 ? GameObject.Find("Player1DemoPoint").transform : GameObject.Find("Player2DemoPoint").transform;

        if (playerIndex == 1)
            Destroy(GameObject.Find("Player2ConnectHelp"));

        SetUIposition();
    }

    public void SetName(string name)
    {
        _playerName.text = name;
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
        GameObject hoist = Instantiate(_hoistPrefab, _choiseTank.transform);
        hoist.transform.localPosition = Vector3.zero;
        hoist.transform.localRotation = Quaternion.identity;
        _nameText.text = tank.Name;

        _content.localPosition = Vector3.zero;
        _tankInfo.richText = true;
        _tankInfo.text = tank.Description.text.Replace("\\n", "\n");
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

    internal void ScrollInfo(float amount)
    {
        _scrollAmount = amount;
    }

    private void Update()
    {
        if (_choiseTank == null)
            return;

        _choiseTank.transform.Rotate(0, -_rotateAmount * 0.4f, 0);
        _content.localPosition += new Vector3(0, _scrollAmount, 0);
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
