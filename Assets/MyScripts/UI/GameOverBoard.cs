using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Data;

namespace Infrastructure.TestMono
{
    [Serializable]
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
        [SerializeField] private int _maxShowLeaders;

        [Space(10)] [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _mainPanel;


        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private string _victoreTextHeader;
        [SerializeField] private string _defeatHederText;
        [SerializeField] private string _teamAHeaderText;
        [SerializeField] private string _teamBHeaderText;

        [Space(10)]
        [Header("Colors")]
        [SerializeField]
        private Color _colorPanelVictory;

        [SerializeField] private Color _colorHeaderVictory;


        [SerializeField] private Color _colorPanelDefeat;
        [SerializeField] private Color _colorHeaderlDefeat;
        [SerializeField] private Color _colorPanelTeam;
        [SerializeField] private Color _colorHeaderTeam;

        [Space(10)]
        [Header("Buttons")]
        [SerializeField]
        private Button _restartButton;
        [SerializeField] 
        private Button _menuButton;

        private int MaxShowLeaders => Mathf.Min(_maxShowLeaders, 10);
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
            ShowScore();
            OnRestart?.Invoke();
        }

        public void ShowPanelWithLeaders(LeadersHolder leadersHolder,
            List<ScoreHolder> playerReference, bool isVictory = false)
        {
            _restartButton.Select();
            
            ShowLeaderList(leadersHolder, playerReference);

            _headerText.text = isVictory ? _victoreTextHeader : _defeatHederText;
            _headerText.color = isVictory ? _colorHeaderVictory : _colorHeaderlDefeat;
            _mainPanel.color = isVictory ? _colorPanelVictory : _colorPanelDefeat;

            _mainPanel.gameObject.SetActive(true);
            _restartButton.Select();
        }

        public void ShowEmptyPanel(List<ID_Settings_CS> playersSettings)
        {
            _restartButton.Select();

            List<ERelationship> playerTeams = playersSettings.Select(x => x.Relationship).ToList();
            playerTeams.Add(ERelationship.TeamB);
            ERelationship winTeam = playerTeams.First();

            _headerText.text = winTeam == ERelationship.TeamA ? _teamAHeaderText : _teamBHeaderText;
            _headerText.color = _colorHeaderTeam;
            _mainPanel.color = _colorPanelTeam;
            HideScore();
            _mainPanel.gameObject.SetActive(true);
            _restartButton.Select();
        }

        private void ShowLeaderList(LeadersHolder scoreList, List<ScoreHolder> playerReference)
        {
            scoreList.Sort();

            FillUpScoreList(scoreList);

            CreateScoreSigns(playerReference);
        }

        private void FillUpScoreList(LeadersHolder scoreList)
        {
            foreach (var scoreHolder in scoreList.Leaders)
                _scoreHolders.Add(scoreHolder);

            int count = _scoreHolders.Count;

            for (int i = 0; i < _maxShowLeaders - count; i++)
            {
                ScoreHolder scoreHolder = new ScoreHolder("------", 0);
                _scoreHolders.Add(scoreHolder);
            }
        }

        private void CreateScoreSigns(List<ScoreHolder> playerReference)
        {
            for (var index = 0; index < _maxShowLeaders; index++)
            {
                var leader = _scoreHolders[index];
                ScorePlane scorePlane = Instantiate(_scorePlanePrefab, _scorePanel);
                scorePlane.SetData(leader, index + 1);

                if (playerReference.Any(x=>leader.Equals(x)))
                    scorePlane.Hightlight();
            }
        }

        private void HideScore() =>
            _scorePanel.gameObject.SetActive(false);

        private void ShowScore() =>
            _scorePanel.gameObject.SetActive(true);

        private void HidePanel() =>
            _mainPanel.gameObject.SetActive(false);
    }
}