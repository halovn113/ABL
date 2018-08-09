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
        Move();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        onUnitLook.RotationUpdate(mousePos);
        RotationControl();
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

        if (Input.GetKey(keys.GetKey("Dash")) && moveState == MoveState.Nope)
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
        transform.position += moveVector.normalized * currentSpeed * Time.deltaTime;

        switch (moveState)
        {
            case MoveState.Dashing:
                //if (moveVector.y != 0)
                //{
                //    transform.position += moveVector.normalized * currentSpeed * 2 * Time.deltaTime;
                //}
                //else
                //{
                //    //if (condition_facingRight)
                //    //{
                //    //    transform.position += new Vector3(1, 0, 0) * currentSpeed * 3 * Time.deltaTime;
                //    //}
                //    //else
                //    //{
                //    //    transform.position += new Vector3(-1, 0, 0) * currentSpeed * 3 * Time.deltaTime;
                //    //}

                //    transform.position += new Vector3(1, 0, 0) * currentSpeed * 3 * Time.deltaTime;

                //}

                transform.position += moveVector.normalized * currentSpeed * 1.5f * Time.deltaTime;
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
        if (_dashTime > 0.3f)
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
        if (transform.localScale.x > 0) // nhìn về bên phải 
        {
            if (mousePos.x > transform.position.x)
            {
                _facingRight = true;
            }
        }
        else
        {
            if (mousePos.x < transform.position.x)
            {
                _facingRight = false;
            }
        }
    }

}
