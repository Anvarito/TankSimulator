using System;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _panel;
    private bool _isEnabled;

    public void UpdateTimer(float seconds)
    {
        if (!_isEnabled && seconds > 0)
        {
            _isEnabled = true;
            _panel.SetActive(true);
        }

        seconds = Mathf.Clamp(seconds, 0, float.MaxValue);
        int sec = Mathf.FloorToInt(seconds);
        var ts = TimeSpan.FromSeconds(sec);
        string formatTime = ts.Minutes + ":" + ts.Seconds;
        _text.text = formatTime;

    }

    public void Init()
    {
        _panel.SetActive(false);
    }
}
