using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.StateMachine
{
    public class MainMenuUIHelper : MonoBehaviour
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

        // https://stackoverflow.com/questions/6394921/get-fields-with-reflection
        private void Start()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                if (field.GetValue(this) == null)
                    Debug.LogWarning($"{field.Name} missing");
        }
    }
}