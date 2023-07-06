using System;
using Infrastructure.Components;
using Infrastructure.Services.Audio;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : UIHelper, IAudioEmitter
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    public event Action OnContinueClick;
    public event Action OnRestartClick;
    public event Action OnSettingsClick;
    public event Action OnExitClick;

    public Slider MusicSlider => _musicSlider;
    public Slider SoundSlider => _soundSlider;

    public event Action<float> OnMusicVolumeChange;
    public event Action<float> OnSoundVolumeChange;

    private IAudioService _audioService;

    void Start()
    {
        _audioService.PlaySound(SoundId.SadTrombone);

        RegisterActions();
    }

    private void OnDestroy()
    {
        _audioService.StopSound();

        UnregisterActions();
    }

    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }

    private void RegisterActions()
    {
        _continueButton.onClick.AddListener(TriggerContinueClick);
        _restartButton.onClick.AddListener(TriggerRestartClick);
        _settingsButton.onClick.AddListener(TriggerSettingsClick);
        _exitButton.onClick.AddListener(TriggerExitClick);
        
        _musicSlider.onValueChanged.AddListener(TriggerMusicVolumeChange);
        _soundSlider.onValueChanged.AddListener(TriggerSoundVolumeChange);
    }

    private void UnregisterActions()
    {
        _continueButton.onClick.RemoveListener(TriggerContinueClick);
        _restartButton.onClick.RemoveListener(TriggerRestartClick);
        _settingsButton.onClick.RemoveListener(TriggerSettingsClick);
        _exitButton.onClick.RemoveListener(TriggerExitClick);
        
        _musicSlider.onValueChanged.RemoveListener(TriggerMusicVolumeChange);
        _soundSlider.onValueChanged.RemoveListener(TriggerSoundVolumeChange);

    }

    private void TriggerContinueClick() =>
        OnContinueClick?.Invoke();

    private void TriggerRestartClick() =>
        OnRestartClick?.Invoke();

    private void TriggerSettingsClick() =>
        OnSettingsClick?.Invoke();

    private void TriggerExitClick() =>
        OnExitClick?.Invoke();

    private void TriggerMusicVolumeChange(float volume) =>
        OnMusicVolumeChange?.Invoke(volume);

    private void TriggerSoundVolumeChange(float volume) =>
        OnSoundVolumeChange?.Invoke(volume);
}