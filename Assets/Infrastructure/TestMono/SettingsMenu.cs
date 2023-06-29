using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Button _screenButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _inputButton;
    [SerializeField] private Button _backButton;

    [Space(20)]
    [SerializeField] private GameObject _settingsPanel;

    [Space(20)]
    [SerializeField] private SettingsAct _screenPanel;
    [SerializeField] private SettingsAct _soundPanel;
    [SerializeField] private SettingsAct _inputPanel;

    public UnityEvent OnBack;
    private void Awake()
    {
        _settingsPanel.SetActive(false);

        _screenButton.onClick.AddListener(OnScreenSetting);
        _soundButton.onClick.AddListener(OnSoundSettings);
        _inputButton.onClick.AddListener(OnInputSettings);
        _backButton.onClick.AddListener(OnPressBack);

    }

    private void OnPressBack()
    {
        if (_settingsPanel.activeInHierarchy)
            _settingsPanel.SetActive(false);

        OnBack?.Invoke();
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject == _screenButton.gameObject
            || _eventSystem.currentSelectedGameObject == _soundButton.gameObject
            || _eventSystem.currentSelectedGameObject == _inputButton.gameObject
            || _eventSystem.currentSelectedGameObject == _backButton.gameObject)
            ShowPanel(null);
    }

    private void OnInputSettings()
    {
        ShowPanel(_inputPanel.gameObject);
        _inputPanel.Launch();
    }

    private void OnSoundSettings()
    {
        ShowPanel(_soundPanel.gameObject);
        _soundPanel.Launch();
    }

    private void OnScreenSetting()
    {
        ShowPanel(_screenPanel.gameObject);
        _screenPanel.Launch();
    }

    internal void Launch()
    {
        _screenButton.Select();
    }

    private void ShowPanel(GameObject panel)
    {
        if (!_settingsPanel.activeInHierarchy)
            _settingsPanel.SetActive(true);

        _screenPanel.gameObject.SetActive(panel == _screenPanel.gameObject);
        _soundPanel.gameObject.SetActive(panel == _soundPanel.gameObject);
        _inputPanel.gameObject.SetActive(panel == _inputPanel.gameObject);
    }
}
