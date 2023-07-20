using System;
using ChobiAssets.PTM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _teamText;
    [SerializeField] private Image _teamImage;

    [Tooltip("Text")]
    [SerializeField] private string _teamAText;
    [SerializeField] private string _teamBText;

    [Tooltip("Colors")]
    [SerializeField] private Color _teamAColor; 
    [SerializeField] private Color _teamBColor; 
    
    public void Init(ID_Settings_CS selfID, string name)
    {
        switch (selfID.Relationship)
        {
            case ERelationship.TeamA:
                _teamText.color = _teamAColor;
                _teamText.text = name;
                break;
            case ERelationship.TeamB:
                _teamText.color = _teamBColor;
                _teamText.text = name;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
