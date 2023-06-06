using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Infrastructure.TestMono
{
    public delegate void OnPressContinue();

    
    public class GameOverBoard : MonoBehaviour
    {
        [SerializeField] private Image _mainPanel;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private string _originText = "�� �������: ";

        [Space(10)]
        [Header("Colors")]
        [SerializeField] private Color _colorPanelVictory;
        [SerializeField] private Color _colorHeaderVictory;
        [SerializeField] private Color _colorPanelDefeat;
        [SerializeField] private Color _colorHeaderlDefeat;

        [Space(10)]
        [Header("Buttons")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartClick);
            _menuButton.onClick.AddListener(MenuClick);
        }

        private void MenuClick()
        {
        }

        private void RestartClick()
        {
            OnPressContinue?.Invoke();
        }

        public void ShowVictory(float score)
        {
            Debug.Log($"Score: {score}");
            _mainPanel.gameObject.SetActive(true);
            _headerText.text = "������!";
            _mainPanel.color = _colorPanelVictory;
            _scoreText.text = _originText + "\n" + score;
        }

        public Action OnPressContinue;
    }
}