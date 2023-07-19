using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScrolling : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    private float _scrollAmount;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        _content.localPosition = Vector3.zero;
    }

    internal void ScrollMove(float amount)
    {
        _scrollAmount = amount;

    }
    private void Update()
    {
        _content.localPosition += new Vector3(0, _scrollAmount, 0);
    }
}
