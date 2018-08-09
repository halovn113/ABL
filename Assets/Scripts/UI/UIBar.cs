using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    public float pixPerValue = 1;
    public float pixPerTime = 1f;

    private Vector2 sizeVector;
    private Action action;

    public void UIBarInit()
    {
        sizeVector = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    public void UpdateBar(float value)
    {
        if (sizeVector.x == value * pixPerValue) return;
        sizeVector.x = value * pixPerValue;
        gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
    }

    public void UpdateBarFixed(float value)
    {
        if (sizeVector.x == value * pixPerValue) return;
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
        if ((int)sizeVector.x < (int)(value * pixPerValue))
        {
            sizeVector.x += pixPerTime;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        else
        {
            sizeVector.x = value * pixPerValue;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
            action = null;
        }
    }

    void UpdateBarDecrease(float value)
    {
        if ((int)sizeVector.x > (int)(value * pixPerValue))
        {
            sizeVector.x -= pixPerTime;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        else
        {
            sizeVector.x = value * pixPerValue;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeVector;
            action = null;
        }
    }

    void Update()
    {
        if (action != null)
        {
            action();
        }
    }
}
