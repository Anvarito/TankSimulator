using System;
using UnityEngine;

namespace Infrastructure.TestMono
{
    public delegate void OnPressContinue();

    public class GameOverBoard : MonoBehaviour
    {
        public void ShowVictory(float score)
        {
            Debug.Log($"Score: {score}");
        }

        public Action OnPressContinue;
    }
}