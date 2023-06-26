using System;
using System.Linq;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.TestMono
{
    public class GamemodeMapHelper : MonoBehaviour
    {
        public Action OnContinueClick;

        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Dropdown _mapDrop;
        [SerializeField] private TMP_Dropdown _modeDrop;
        private IStaticDataService _dataService;
        private IProgressService _progress;
        private IInputService _inputService;

        private void OnDestroy() =>
            _continueButton.onClick.RemoveListener(ContinueAction);


        public void Construct(IProgressService progress, IStaticDataService dataService, IInputService inputService)
        {
            _inputService = inputService;
            _progress = progress;
            _dataService = dataService;
            Initialize();
        }

        private void Initialize()
        {
            _continueButton.onClick.AddListener(ContinueAction);

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
                _mapDrop.options.Add(new TMP_Dropdown.OptionData(_dataService.Levels[id].SceneName));
        }

        private void ContinueAction()
        {
            _progress.Progress.WorldData.ModeId = _dataService.Mods.ToList()[_modeDrop.value].Key;
            _progress.Progress.WorldData.LevelId = _dataService.Levels.ToList()[_mapDrop.value].Key;

            _progress.Progress.WorldData.Level = _dataService.Levels.ToList()[_mapDrop.value].Value.SceneName;
            OnContinueClick?.Invoke();
        }
    }
}