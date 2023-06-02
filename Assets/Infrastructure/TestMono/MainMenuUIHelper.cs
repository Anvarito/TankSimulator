using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.TestMono
{
    public class MainMenuUIHelper : UIHelper 
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _trainButton;
        [SerializeField] private Button _exitButton;

        public UnityEvent OnContinueButtonPress => _continueButton.onClick;
        public UnityEvent OnSettingsPress => _settingsButton.onClick;
        public UnityEvent OnOnNewGameButtonPress => _newGameButton.onClick;
        public UnityEvent OnInfoButtonPress => _infoButton.onClick;
        public UnityEvent OnTrainButtonPress => _trainButton.onClick;
        public UnityEvent OnExitButtonPress => _exitButton.onClick;
    }
}