using System;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public void UpdateTimer(float seconds)
    {
        int sec = Mathf.FloorToInt(seconds);
        var ts = TimeSpan.FromSeconds(sec);
        string formatTime = ts.Minutes + ":" + ts.Seconds;
        _text.text = formatTime;
    }
}
