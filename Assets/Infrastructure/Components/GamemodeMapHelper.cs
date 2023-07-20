using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.Components
{
    public class GamemodeMapHelper : MonoBehaviour
    {
        [System.Serializable]
        public class MapeModeHolder
        {
            public string name = "";
            public Sprite _mapSprite;
            public List<Sprite> _modsSprites;
            public string MapDesription = "";
        }

        [SerializeField] private List<MapeModeHolder> _modeMapHolders;
        [SerializeField] private List<string> _modeDescription;

        [SerializeField] private Image _mapsView;
        [SerializeField] private Image _modsView;

        [SerializeField] private TextMeshProUGUI _modsText;
        [SerializeField] private TextMeshProUGUI _mapsText;

        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Dropdown _mapDrop;
        [SerializeField] private TMP_Dropdown _modeDrop;

        private IStaticDataService _dataService;
        private IProgressService _progress;
        private IInputService _inputService;

        private int _mapLastValue = -1;
        private int _modeLastValue = -1;

        public Action OnContinueClick;
        public Action OnBackClick;

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(BackAction);
            _continueButton.onClick.RemoveListener(ContinueAction);
        }

        public void Construct(IProgressService progress, IStaticDataService dataService, IInputService inputService)
        {
            _inputService = inputService;
            _progress = progress;
            _dataService = dataService;
            Initialize();
        }

        private void Update()
        {
            if (_mapDrop.value != _mapLastValue)
            {
                _mapLastValue = _mapDrop.value;
                _modeLastValue = -1;
                _modeDrop.value = 0;
            }

            if (_modeDrop.value != _modeLastValue)
            {
                _modeLastValue = _modeDrop.value;
                ChangeInfo();
            }
        }

        private void ChangeInfo()
        {
            _mapsView.sprite = _modeMapHolders[_mapLastValue]._mapSprite;
            _modsView.sprite = _modeMapHolders[_mapLastValue]._modsSprites[_modeLastValue];

            _mapsText.text = _modeMapHolders[_mapLastValue].MapDesription;
            _modsText.text = _modeDescription[_modeLastValue];
        }

        private void Initialize()
        {
            _continueButton.onClick.AddListener(ContinueAction);
            _backButton.onClick.AddListener(BackAction);

            InitLevelDropdown();
            InitModsDropdown();
        }



        private void InitModsDropdown()
        {
            _modeDrop.options.Clear();
            foreach (GamemodeId id in _dataService.Mods.Keys)
            {
                if (_dataService.Mods[id].PlayerCount <= _inputService.PlayerConfigs.Count)
                    _modeDrop.options.Add(new TMP_Dropdown.OptionData(_dataService.Mods[id].Name));
            }
        }

        private void InitLevelDropdown()
        {
            _mapDrop.options.Clear();
            foreach (LevelId id in _dataService.Levels.Keys)
                _mapDrop.options.Add(new TMP_Dropdown.OptionData(_dataService.Levels[id].InGameName));
        }

        private void ContinueAction()
        {
            _progress.Progress.WorldData.ModeId = _dataService.Mods.ToList()[_modeDrop.value].Key;
            _progress.Progress.WorldData.LevelId = _dataService.Levels.ToList()[_mapDrop.value].Key;

            _progress.Progress.WorldData.Level = _dataService.Levels.ToList()[_mapDrop.value].Value.SceneName;
            OnContinueClick?.Invoke();
        }
        private void BackAction()
        {
            OnBackClick?.Invoke();
        }
    }
}