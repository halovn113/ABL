using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private PlayerControl _playerControl;
    public PlayerStats stats;

    public GameObject bullet;

    [HideInInspector]
    public float normalMoveSpeed;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentStamina;
    [HideInInspector]
    public float currentVitality;

    // sprite của từng state: 0: up, 1: down, 2: left, 3: down
    public Sprite[] spriteStates;
    public AttackType attackType;


    public float maxStamina;
    public float maxVitality;

    public Player instance;

    public enum StaminaRecoverState
    {
        CanRecover,
        Wait,
        None,
    }

    private StaminaRecoverState _staminaState;

    private bool _canRecoverStamina;
    private float _recoverStaminaTimer;

    private float _maxStaminaRecoverTime;

    void Start()
    {
        PlayerInit();
    }

    public void PlayerInit()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        _playerControl = gameObject.GetComponent<PlayerControl>();
        stats = gameObject.GetComponent<PlayerStats>();
        _playerControl.Init();
        StatPhysical(stats.physical);
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentVitality = maxVitality;
        _staminaState = StaminaRecoverState.None;
        //Debug.Log("cur sta " + currentStamina);
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
        UpdateStats();
    }

    public void StatStrength(int value)
    {
        if (value <= 0) return;
        stats.strength = value;
        UpdateStats();

    }

    public void StatEndurable(int value)
    {
        if (value <= 0) return;
        stats.endurable = value;
        UpdateStats();

    }

    public void UpdateStats()
    {
        maxHealth = stats.physical * stats.healthPer + stats.strength * stats.healthPer / 4;
        maxVitality = stats.physical * stats.healthPer / 4 + stats.endurable * stats.healthPer + stats.strength * stats.healthPer / 5;
        maxStamina = stats.physical * stats.healthPer / 4 + stats.endurable * stats.healthPer + stats.strength * stats.healthPer / 5;
        GameDirector.instance.UIDirector.uiPlayer.Stamina.pixPerTime = stats.staminaRecoverPerSec;
    }

    public void UpdateStamina(float value)
    {
        //Debug.Log("update stamina");
        if (value == 0) return;
        //_canRecoverStamina = false;
        //this.StopInvoke(0);
        _staminaState = StaminaRecoverState.None;
        Debug.Log("state " + _staminaState + " " + value + "cur sta " +  currentStamina);
        //GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
        //GameDirector.instance.UIDirector.uiPlayer.Stamina.StopUpdate();
        if (currentStamina + value >= maxStamina)
        {
            currentStamina = maxStamina;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
        }

        if (currentStamina + value <= 0)
        {
            currentStamina = 0;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
            _maxStaminaRecoverTime = 3.0f;
            _staminaState = StaminaRecoverState.Wait;
            //this.Invoke(StartAutoRecoveStamina, 3.0f);
            return;
        }

        currentStamina += value;
        GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBarFixed(currentStamina);
        _maxStaminaRecoverTime = 2.0f;
        _staminaState = StaminaRecoverState.Wait;
    }

    public void RecoverStamina()
    {
        UpdateStamina(stats.staminaRecoverPerSec);
        //Debug.Log("hello there");
    }

    public void WaitForRecoverStamina()
    {
        //if (_recoverStaminaTimer > _maxStaminaRecoverTime)
        //{
        //    _recoverStaminaTimer = 0;
        //    //_canRecoverStamina = true;
        //    _staminaState = StaminaRecoverState.CanRecover;
        //}
        //else
        //{
        //    _recoverStaminaTimer += Time.deltaTime;
        //}
        if (GameDirector.instance.UIDirector.uiPlayer.Stamina.IsUpdateDone())
        {
            //this.Invoke(StartAutoRecoveStamina, 1.0f);

            if (_recoverStaminaTimer > _maxStaminaRecoverTime)
            {
                _recoverStaminaTimer = 0;
                //_canRecoverStamina = true;
                _staminaState = StaminaRecoverState.CanRecover;
            }
            else
            {
                _recoverStaminaTimer += Time.deltaTime;
            }
        }

    }

    void Update()
    {
        //if (_canRecoverStamina)
        //{
        //    _RecoverStamina();
        //}
        switch (_staminaState)
        {
            case StaminaRecoverState.Wait:
                WaitForRecoverStamina();
                break;
            case StaminaRecoverState.CanRecover:
                _RecoverStamina();
                break;
        }
    }

    void FixedUpdate()
    {
        ChangeSpriteFacing();
    }


    void StartAutoRecoveStamina()
    {
        //_canRecoverStamina = true;
        _staminaState = StaminaRecoverState.CanRecover;
    }

    void _RecoverStamina()
    {
        //if (_staminaState != StaminaRecoverState.CanRecover) return;
        if (currentStamina + stats.staminaRecoverPerSec <= maxStamina)
        {
            currentStamina += stats.staminaRecoverPerSec;
            GameDirector.instance.UIDirector.uiPlayer.Stamina.UpdateBar(currentStamina);
        }
        else
        {
            //Debug.Log("cur sta " + currentStamina);
            currentStamina = maxStamina;
            _staminaState = StaminaRecoverState.None;
            //_canRecoverStamina = false;
            //_recoverStamina = null;
        }
    }

    public void Attack()
    {
        switch (unitFacing)
        {
            case Facing.Up:
                break;
            case Facing.Down:
                break;
            case Facing.Left:
                break;
            case Facing.Right:
                break;
        }
    }

    public void ChangeSpriteFacing()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteStates[(int)unitFacing];
    }

}

public enum AttackType
{
    Slash,
    Thrust,
    Shoot,
    Cast,
}
