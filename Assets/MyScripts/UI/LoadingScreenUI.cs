using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private Image _loadLine;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void LoadProcess(float value)
    {
        _loadLine.fillAmount = value;
    }

    public void Hide()
    {
        Destroy(gameObject);
    }
}
