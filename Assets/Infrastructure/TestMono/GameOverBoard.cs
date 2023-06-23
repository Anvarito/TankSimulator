using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChobiAssets.PTM;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Infrastructure.TestMono
{
    public class ScoreHolder
    {
        public string Name;
        public float Points;

        public ScoreHolder(string name, float points)
        {
            Name = name;
            Points = points;
        }
    }
    public class GameOverBoard : MonoBehaviour
    {
        
        [SerializeField] private ScorePlane _scorePlanePrefab;
        [SerializeField] private Transform _scorePanel;
        [SerializeField] private int _maxShowLeaders = 7;

        [Space(10)]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _mainPanel;
        [SerializeField] private TextMeshProUGUI _headerText;
        //[SerializeField] private TextMeshProUGUI _scoreText;
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

        private List<ScoreHolder> _scoreHolders = new List<ScoreHolder>();

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
            HidePanel();
            OnRestart?.Invoke();
        }

        public void ShowVictoryPanel(Dictionary<string, float> scoreList, ScoreHolder playerReference)
        {
            Debug.Log($"Score: {playerReference}");

            ShowLeaderList(scoreList, playerReference);

            _mainPanel.gameObject.SetActive(true);
            _headerText.text = "Победа!";
            _headerText.color = _colorHeaderVictory;
            _mainPanel.color = _colorPanelVictory;
            //_scoreText.text = _originText + "\n" + score;
        }


        public void ShowDefeatPanel(Dictionary<string, float> scoreList, ScoreHolder playerReference)
        {
            Debug.Log($"Score: {playerReference}");

            ShowLeaderList(scoreList, playerReference);

            _mainPanel.gameObject.SetActive(true);
            _headerText.text = "Поражение!";
            _headerText.color = _colorHeaderlDefeat;
            _mainPanel.color = _colorPanelDefeat;
            //_scoreText.text = _originText + "\n" + score;
        }

        private void ShowLeaderList(Dictionary<string, float> scoreList, ScoreHolder playerReference)
        {
            foreach (var i in scoreList)
            {
                ScoreHolder scoreHolder = new ScoreHolder(i.Key, i.Value);
                _scoreHolders.Add(scoreHolder);
            }

            for(int i =0; i < _maxShowLeaders - _scoreHolders.Count; i++)
            {
                ScoreHolder scoreHolder = new ScoreHolder("Пусто", 0);
                _scoreHolders.Add(scoreHolder);
            }

            BubbleSortList();

            for (int i = _maxShowLeaders - 1; i >= 0; i--)
            {
               var current = _scoreHolders.Count - _maxShowLeaders + i;
                ScorePlane scorePlane = Instantiate(_scorePlanePrefab, _scorePanel);
                scorePlane.SetData(_scoreHolders[current]);
                if (_scoreHolders[current].Name == playerReference.Name)
                    scorePlane.Hightlight();
            }

        }

        public void BubbleSortList()
        {
            var count = _scoreHolders.Count;
            for (int i = 0; i < count - 1; i++)
                for (int j = 0; j < count - i - 1; j++)
                    if (_scoreHolders[j].Points > _scoreHolders[j + 1].Points)
                    {
                        var tempVar = _scoreHolders[j];
                        _scoreHolders[j] = _scoreHolders[j + 1];
                        _scoreHolders[j + 1] = tempVar;
                    }
        }

        private void HidePanel()
        {
            _mainPanel.gameObject.SetActive(false);
        }

    }
}