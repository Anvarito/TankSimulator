using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.Components
{
    public class MainMenuUIHelper : UIHelper 
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _trainButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundsSlider;

        public UnityEvent OnContinueButtonPress => _continueButton.onClick;
        public UnityEvent OnSettingsPress => _settingsButton.onClick;
        public UnityEvent OnOnNewGameButtonPress => _newGameButton.onClick;
        public UnityEvent OnInfoButtonPress => _infoButton.onClick;
        public UnityEvent OnTrainButtonPress => _trainButton.onClick;
        public UnityEvent OnExitButtonPress => _exitButton.onClick;
        public Slider MusicSlider => _musicSlider;
        public Slider SoundsSlider => _soundsSlider;
        public Slider.SliderEvent OnMusicSlider => _musicSlider.onValueChanged;
        public Slider.SliderEvent OnSoundsSlider => _soundsSlider.onValueChanged;
    }
}