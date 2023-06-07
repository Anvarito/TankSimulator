using System;
using System.Linq;
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

        private void OnDestroy() =>
            _continueButton.onClick.RemoveListener(InvokeAction);


        public void Construct(IProgressService progress, IStaticDataService dataService)
        {
            _progress = progress;
            _dataService = dataService;
            Initialize();
        }

        private void Initialize()
        {
            _continueButton.onClick.AddListener(InvokeAction);

            InitLevelDropdown();
            InitModsDropdown();
        }

        private void InitModsDropdown()
        {
            _modeDrop.options.Clear();
            foreach (GamemodeId id in _dataService.Mods.Keys)
                _modeDrop.options.Add(new TMP_Dropdown.OptionData(_dataService.Mods[id].Name));
        }

        private void InitLevelDropdown()
        {
            _mapDrop.options.Clear();
            foreach (LevelId id in _dataService.Levels.Keys)
                _mapDrop.options.Add(new TMP_Dropdown.OptionData(_dataService.Levels[id].Scene.name));
        }

        private void InvokeAction()
        {
            _progress.Progress.WorldData.Mode = _dataService.Mods.ToList()[_modeDrop.value].Key;
            _progress.Progress.WorldData.Level = _dataService.Levels.ToList()[_mapDrop.value].Value.Scene.name;
            OnContinueClick?.Invoke();
        }
    }
}