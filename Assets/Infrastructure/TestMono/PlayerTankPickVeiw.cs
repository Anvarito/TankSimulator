using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankPickVeiw : MonoBehaviour
{
    [SerializeField] private TankPickerUIHelper _tankPickerUIHelper;
    [SerializeField] private Image _faceImage;

    [SerializeField] private Sprite _tank1;
    [SerializeField] private Sprite _tank2;
    [SerializeField] private Sprite _tank3;

    private int _choiseIndex = 0;

    private void Awake()
    {
        _tankPickerUIHelper.OnRight.AddListener(MoveToRight);
        _tankPickerUIHelper.OnLeft.AddListener(MoveToLeft);
    }

    private void MoveToLeft()
    {
        print("Left");
        _choiseIndex--;

    }

    private void MoveToRight()
    {
        print("Right");
        _choiseIndex++;

    }
}
