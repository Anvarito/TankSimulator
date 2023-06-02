using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerInMenu : MonoBehaviour
{
    private int playerIndex;

    [SerializeField] private List<Button> _tankButtons;
    [SerializeField] private InputSystemUIInputModule _inputSystemUIInput ;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;
    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    public InputSystemUIInputModule GetInputSystemUIInput => _inputSystemUIInput;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    //Call from unity event in inspector
    public void SelectTank(int tankIndex)
    {
        if(!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.SetPlayerTank(playerIndex, tankIndex);
        readyPanel.SetActive(true);
        readyButton.interactable = true;
        menuPanel.SetActive(false);
        readyButton.Select();
    }

    //Call from unity event in inspector
    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
