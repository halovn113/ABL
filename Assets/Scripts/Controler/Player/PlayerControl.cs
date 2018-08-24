using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    enum MoveState
    {
        Nope,
        Dashing,
        AfterDash,
    }

    MoveState moveState;
    AttackState attackCon;

    public float maxSpeed;
    public KeyBinding keys;

    // test fields
    public OnAttack onAttack;

    private Vector3 mousePos;
    private OnUnitLook onUnitLook;
    private float _dashTime;
    // Use this for initialization

    private Vector3 moveVector;
    private bool _facingRight;
    private float _afterDash;

    private float normalSpeed;
    private float currentSpeed;
    private Vector3 _screenPoint;
    private Vector3 _direction;
    private float _angle;

    public void Init()
    {
        keys = GetComponent<KeyBinding>();
        keys.Init();
        moveVector = new Vector3(0, 0, 0);
        normalSpeed = gameObject.GetComponent<PlayerStats>().moveSpeed;
        currentSpeed = normalSpeed;
        onUnitLook = gameObject.GetComponent<OnUnitLook>();
        onUnitLook.SetTarget(mousePos);
        _dashTime = 0;
        _afterDash = 0;
        moveState = MoveState.Nope;
        onAttack.Init();
        attackCon = onAttack.GetAttackState();
    }

    void Start ()
    {
        
    }

    void LateUpdate()
    {
        Control();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (gameObject.GetComponent<Player>().attackType == AttackType.Shoot)
        {
            onUnitLook.RotationUpdate(mousePos);
        }
        RotationControl();
    }

    void FixedUpdate()
    {
        Move();

    }

    void Control()
    {
        if (Input.GetKey(keys.GetKey("Up")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.y = 1;
        }
        else if (Input.GetKey(keys.GetKey("Down")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.y = -1;
        }
        else
        {
            moveVector.y = 0;
        }

        if (Input.GetKey(keys.GetKey("Left")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.x = -1;
        }
        else if (Input.GetKey(keys.GetKey("Right")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.x = 1;
        }
        else
        {
            moveVector.x = 0;
        }

        if (Input.GetKey(keys.GetKey("Dash")) && moveState == MoveState.Nope && gameObject.GetComponent<Player>().currentStamina > 0)
        {
            gameObject.GetComponent<Player>().UpdateStamina(-20);
            moveState = MoveState.Dashing;
        }

        if (Input.GetKey(keys.GetKey("Attack1")) && onAttack.GetAttackState() == AttackState.Nope)
        {
            onAttack.SetAttackState(AttackState.Attacking);
        }
    }

    void Move()
    {
        transform.GetComponent<Rigidbody2D>().velocity = moveVector.normalized * currentSpeed; 
        switch (moveState)
        {
            case MoveState.Dashing:             
                transform.GetComponent<Rigidbody2D>().velocity = moveVector.normalized * 2.5f * currentSpeed;
                DashCondition();
                break;

            case MoveState.AfterDash:
                if (_afterDash > 0.5f)
                {
                    _afterDash = 0;
                    moveState = MoveState.Nope;

                }
                else
                {
                    _afterDash += Time.deltaTime;
                }
                break;
        }
    }

    void DashCondition()
    {
        if (_dashTime > 0.2f)
        {
            _dashTime = 0;
            moveState = MoveState.AfterDash;

        }
        else
        {
            _dashTime += Time.deltaTime;
        }
    }

    void RotationControl()
    {
        _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        _direction = (Input.mousePosition - _screenPoint).normalized;
        _angle = Utility.Angle360(Vector3.right, _direction);

        if (_angle >= 45 && _angle <= 135)
        {
            gameObject.GetComponent<Player>().unitFacing = Facing.Up;
        }
        else if (_angle > 135 && _angle < 225)
        {
            gameObject.GetComponent<Player>().unitFacing = Facing.Left;
        }
        else if (_angle >= 225 && _angle <= 315)
        {
            gameObject.GetComponent<Player>().unitFacing = Facing.Down;
        }
        else if (_angle > 315 || _angle < 45)
        {
            gameObject.GetComponent<Player>().unitFacing = Facing.Right;
        }

    }

}
