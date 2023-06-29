using UnityEngine;
using TMPro;
using ChobiAssets.PTM;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private ID_Settings_CS _selfID;
    internal void UpdateScore(float score)
    {
        _text.text = score.ToString();
    }

    internal void Init(ID_Settings_CS selfID)
    {
        _selfID = selfID;
        _text.text = "0";
    }
}
