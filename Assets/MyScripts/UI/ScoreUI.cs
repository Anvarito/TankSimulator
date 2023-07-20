using UnityEngine;
using TMPro;
using ChobiAssets.PTM;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    internal void UpdateScore(float score)
    {
        _text.text = score.ToString();
    }

    internal void Init()
    {
        _text.text = "0";
    }
}
