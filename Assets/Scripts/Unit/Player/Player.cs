using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private PlayerControl _playerControl;
    public PlayerStats stats;


    [HideInInspector]
    public float normalMoveSpeed;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentStamina;
    [HideInInspector]
    public float currentVitality;

    public float maxStamina;
    public float maxVitality;

    private Action _recoverStamina;

    private bool _canRecoverStamina;

    void Start()
    {
        PlayerInit();
    }

    public void PlayerInit()
    {
        _playerControl = gameObject.GetComponent<PlayerControl>();
        stats = gameObject.GetComponent<PlayerStats>();
        _playerControl.Init();
        StatPhysical(stats.physical);
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentVitality = maxVitality;
        //this.InvokeRepeating(() => { Test1(); }, 2, 1);
        //this.StopInvokeRepeating(10); 
    }

    public override void HealthUpdate(float number)
    {
        base.HealthUpdate(number);
        GameDirector.instance.UIDirector.uiPlayer.Health.UpdateBarFixed(currentHealth);
    }

    public void StatPhysical(int value)
    {
        if (value <= 0) return;
        stats.physical = value;
        maxHealth = stats.physical * stats.healthPer;
        maxVitality = stats.physical * stats.healthPer / 4;
        maxStamina = stats.physical * stats.healthPer / 4;
    }

    public void StatStrength(int value)
    {
        if (value <= 0) return;
        stats.strength = value;

    }

    public void UpdateStamina(float value)
    {
        //Debug.Log("update stamina");
        if (value == 0) return;

        this.StopInvoke(0);
        _recoverStamina = null;

        _canRecoverStamina = false;

        if (currentStamina + value >= maxStamina)
        {
            currentStamina = maxStamina;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
        }

        if (currentStamina + value <= 0)
        {
            currentStamina = 0;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
            //this.InvokeRepeating(RecoverStamina, 1.0f, 1 / 60);

        }

        currentStamina += value;
        GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
        this.Invoke(StartAutoRecoveStamina, 3.0f);
        //this.InvokeRepeating(RecoverStamina, 2.0f, 1 / 60);
    }

    public void RecoverStamina()
    {
        UpdateStamina(stats.staminaRecoverPerSec);
        //Debug.Log("hello there");
    }


    void Update()
    {
        if (_recoverStamina != null)
        {
            _recoverStamina();
        }

        //if (_canRecoverStamina)
        //{
        //    _RecoverStamina();
        //}
    }

    void StartAutoRecoveStamina()
    {
        //Debug.Log("start recover stamina");
        _recoverStamina = _RecoverStamina;
        //_canRecoverStamina = true;
    }

    void _RecoverStamina()
    {
        if (currentStamina + stats.staminaRecoverPerSec <= maxStamina)
        {
            currentStamina += stats.staminaRecoverPerSec;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
        }
        else
        {
            currentStamina = maxStamina;
            //_canRecoverStamina = false;
            _recoverStamina = null;
        }
    }
}
