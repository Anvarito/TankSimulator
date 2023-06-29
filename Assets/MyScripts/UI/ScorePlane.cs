using UnityEngine;
using TMPro;
using Infrastructure.TestMono;

public class ScorePlane : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _hightLightColor;
    private Color _currentColor;
    private Color _targetColor;
    private float _hlDur = 0.7f;
    private float _hlTimer = 0;
    private bool _isHlight = false;

    public ScoreHolder ScoreHolder { get; private set; }
    public void SetData(ScoreHolder scoreHolder)
    {
        ScoreHolder = scoreHolder;
        _text.text = scoreHolder.Name.ToUpper() + ": " + scoreHolder.Points.ToString() + " очков";
    }

    internal void Hightlight()
    {
        _isHlight = true;
        _currentColor = _defaultColor;
        _targetColor = _hightLightColor;
    }

    private void Update()
    {
        if (_isHlight)
        {
            _hlTimer += Time.deltaTime;
            float alpha = _hlTimer / _hlDur;
            _text.color = Color.Lerp(_currentColor, _targetColor, alpha);
            if (alpha >= 1)
            {
                _hlTimer = 0;
                var tempColor = _currentColor;
                _currentColor = _targetColor;
                _targetColor = tempColor;
            }
        }
    }
}
