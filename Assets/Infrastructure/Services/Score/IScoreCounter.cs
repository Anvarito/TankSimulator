using System;
using System.Collections.Generic;
using ChobiAssets.PTM;

namespace Infrastructure.Services.Score
{
    public interface IScoreCounter : IService
    {
        public float ScorePlayerOne { get; }
        public float ScorePlayerTwo { get; }

        public List<float> Scores { get; }
        public Action<int> OnEnemiesDestroyed { get; set; }
        public void LoadData();
        void AddPlayerIndex(ID_Settings_CS id, int index);
        public int GetIndexPlayer(ID_Settings_CS player);
    }
}