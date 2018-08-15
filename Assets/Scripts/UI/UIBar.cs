﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    public float pixPerValue = 1;
    public float pixPerTime = 1f;

    private Vector2 sizeVector;
    private Action action;
    private bool _updateDone;

    public void UIBarInit()
    {
        sizeVector = gameObject.GetComponent<RectTransform>().sizeDelta;
        _updateDone = false;
    }

    public void UpdateBar(float value)
    {
        if (sizeVector.x == value * pixPerValue) return;
        _updateDone = false;
        sizeVector.x = value * pixPerValue;
        gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
    }

    public void UpdateBarFixed(float value)
    {
        if (sizeVector.x == value * pixPerValue) return;
        _updateDone = false;
        if (sizeVector.x > (value * pixPerValue))
        {
            action = () =>
            {
                UpdateBarDecrease(value);
            };
        }
        else
        {
            action = () =>
            {
                UpdateBarIncrease(value);
            };
        }
       
    }

    void UpdateBarIncrease(float value)
    {
        if ((int)sizeVector.x < (int)(value * pixPerValue) && !_updateDone)
        {
            sizeVector.x += pixPerTime;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        else
        {
            sizeVector.x = value * pixPerValue;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
            action = null;
            _updateDone = true;
        }
    }

    void UpdateBarDecrease(float value)
    {
        if ((int)sizeVector.x > (int)(value * pixPerValue) && !_updateDone)
        {
            sizeVector.x -= pixPerTime;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        else
        {
            sizeVector.x = value * pixPerValue;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
            action = null;
            _updateDone = true;
        }
    }

    void Update()
    {
        if (action != null)
        {
            action();
        }
    }

    public bool IsUpdateDone()
    {
        return _updateDone;
    }

    public void StopUpdate()
    {
        _updateDone = true;
    }
}
