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
        if (value == 0) return;

        this.StopInvokeRepeating(0);
        if (currentStamina + value >= maxStamina)
        {
            currentStamina = maxStamina;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
            return;
        }

        if (currentStamina + value <= 0)
        {
            currentStamina = 0;
            this.InvokeRepeating(RecoverStamina, 1, 0.2f);
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
            return;
        }

        currentStamina += value;
        this.InvokeRepeating(RecoverStamina, 0.5f, 0.1f);
        GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
    }

    public void RecoverStamina()
    {
        UpdateStamina(maxStamina - currentStamina);
        Debug.Log("hello there");
    }
}
