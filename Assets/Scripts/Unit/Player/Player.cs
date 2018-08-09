using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private PlayerControl _playerControl;
    private PlayerStats _stats;


    [HideInInspector]
    public float normalMoveSpeed;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentStamina;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentVitality;

    public float maxStamina;
    public float maxVitality;

    void Start()
    {
        _playerControl = gameObject.GetComponent<PlayerControl>();
        _stats = gameObject.GetComponent<PlayerStats>();
        _playerControl.Init();
    }

    public PlayerStats GetStats()
    {
        return _stats;
    }

    public override void HealthApply(float number)
    {
        base.HealthApply(number);
        GameDirector.instance.UIDirector.uiPlayer.Health.UpdateBarFixed(health);
    }
}
