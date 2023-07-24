using System;
using ChobiAssets.PTM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _teamText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _teamImage;
    [SerializeField] private GameObject _panelName;

    [Tooltip("Text")]
    [SerializeField] private string _teamAText;
    [SerializeField] private string _teamBText;

    [Tooltip("Colors")]
    [SerializeField] private Color _teamAColor; 
    [SerializeField] private Color _teamBColor; 
    
    public void Init(ID_Settings_CS selfID, string name)
    {
        _nameText.text = name;
        _panelName.SetActive(!name.Equals("") && !_panelName.activeSelf);

        switch (selfID.Relationship)
        {
            case ERelationship.TeamA:
                _teamText.color = _teamAColor;
                _nameText.color = _teamAColor;
                _teamText.text = _teamAText;
                break;
            case ERelationship.TeamB:
                _teamText.color = _teamBColor;
                _nameText.color = _teamBColor;
                _teamText.text = _teamBText;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
