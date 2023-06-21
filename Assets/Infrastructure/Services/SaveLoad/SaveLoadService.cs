using System.Linq;
using Infrastructure.Data;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IProgressService _progressService;
        private readonly IFactories _factories;
        private const string ProgressKey = "Progress";

        public SaveLoadService(IProgressService progressService,IFactories factories)
        {
            _progressService = progressService;
            _factories = factories;
        }

        public void SaveProgress()
        {
            foreach (var progressWriter in _factories.All.Values.SelectMany(factory => factory.ProgressWriters))
                progressWriter.UpdateProgress(_progressService.Progress);

            PlayerPrefs.SetString(ProgressKey,_progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress() => 
            PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();

        public void CleanUp()
        {
        }
    }
}