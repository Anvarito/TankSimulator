using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChobiAssets.PTM;
using UnityEngine.Events;

namespace Infrastructure.TestMono
{
    public delegate void OnPressContinue();

    
    public class GameOverBoard : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _mainPanel;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private string _originText = "Вы набрали: ";

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

        public UnityAction OnExitMenu;
        public UnityAction OnRestart;
        public void Awake()
        {
            _restartButton.onClick.AddListener(RestartClick);
            _menuButton.onClick.AddListener(MenuClick);
        }

        private void MenuClick()
        {
            OnExitMenu?.Invoke();
        }

        [ContextMenu("Restart")]
        private void RestartClick()
        {
            OnRestart?.Invoke();
        }

        public void ShowVictoryPanel(float score)
        {
            Debug.Log($"Score: {score}");
            _mainPanel.gameObject.SetActive(true);
            _headerText.text = "Победа!";
            _headerText.color = _colorHeaderVictory;
            _mainPanel.color = _colorPanelVictory;
            _scoreText.text = _originText + "\n" + score;
        }


        internal void ShowDefeatPanel(float score)
        {
            Debug.Log($"Score: {score}");
            _mainPanel.gameObject.SetActive(true);
            _headerText.text = "Поражение!";
            _headerText.color = _colorHeaderlDefeat;
            _mainPanel.color = _colorPanelDefeat;
            _scoreText.text = _originText + "\n" + score;
        }

    }
}