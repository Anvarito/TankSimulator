using UnityEngine;
using TMPro;
using ChobiAssets.PTM;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _panel;
    internal void UpdateScore(float score)
    {
        _panel.SetActive(!_panel.activeSelf);
        _text.text = score.ToString();
    }

    internal void Init()
    {
        _text.text = "0";
        _panel.SetActive(false);
    }
}
