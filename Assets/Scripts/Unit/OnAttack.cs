using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum tạm sẽ update trong ScriptableObject của EnumGenerator
public enum AttackType
{
    Slash,
    Thrust,
    Shoot,
    Cast,
}

public enum AttackState
{
    Nope,
    Attacking,
    AfterAttack,
}

public class OnAttack : MonoBehaviour
{

    // public
    public Collider2D attackTrigger; // test collider
    public GameObject weapon; // test melee trước
    private AttackState attackState;
    
    public AttackType attackType;
    public float timeAttack = 0.5f; // 0.5 chỉ để test
    public float nextAttack = 0.5f; // 0.5 để test

    // private
    private float attackTimer;
    private float afterATimer;
    private Vector3 aimingPos;
    //private ObjectPooler objPooler; // test

    private Vector3 screenPoint;
    private Vector3 direction;

    public void Init()
    {
        //attackTrigger = GetComponent<Collider2D>();
        attackTimer = 0;
        afterATimer = 0;
        weapon.SetActive(false);
        attackTrigger.enabled = false;
        aimingPos = new Vector3(0, 0, 0);
        //objPooler = ObjectPooler.Instance;
    }

    [ContextMenu("Get collider")]
    public void EditorInit()
    {
        attackTrigger = GetComponentInChildren<Collider2D>();        
    }

    public void SetAttackState(AttackState state)
    {
        attackState = state;
        SetAttack();
        aimingPos = weapon.transform.position;
    }

    public void Attack()
    {
        attackState = AttackState.Attacking;
        attackTimer = timeAttack;
    }

    public AttackState GetAttackState()
    {
        return attackState;
    }


    void FixedUpdate()
    {
        switch (attackState)
        {
            case AttackState.Attacking:
                AttackWork();
                break;
            case AttackState.AfterAttack:
                AfterAttack();
                break;
        }
    }


    public void AttackWork()
    {
        switch (attackType)
        {
            case AttackType.Slash:
                //weapon.transform.position = aimingPos;
                ActiveTrigger();
                AttackSlash();
                break;

            case AttackType.Thrust:
                break;

            case AttackType.Shoot:
                AttackSlash();
                break;

            case AttackType.Cast:
                break;
        }
    }

    void AttackSlash()
    {
        //if (attackTimer > 0)
        //{
        //    attackTimer -= Time.deltaTime;
        //}
        //else
        //{
        //    weapon.SetActive(false);
        //    attackState = AttackState.AfterAttack;
        //}

        if (attackTimer > timeAttack)
        {
            attackTimer = 0;
            weapon.SetActive(false);
            DeactiveTrigger();
            attackState = AttackState.AfterAttack;
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    void AfterAttack()
    {
        if (afterATimer > nextAttack)
        {
            afterATimer = 0;
            attackState = AttackState.Nope;
        }
        else
        {
            afterATimer += Time.deltaTime;
        }
    }

    public void ActiveTrigger()
    {
        attackTrigger.enabled = true;
    }

    public void DeactiveTrigger()
    {
        attackTrigger.enabled = false;
    }

    public void SetAttack()
    {
        switch (attackType)
        {
            case AttackType.Slash:
                weapon.SetActive(true);
                break;
            case AttackType.Thrust:
                break;
            case AttackType.Shoot:
                screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                direction = (Input.mousePosition - screenPoint).normalized;
                //GameObject bullet = objPooler.SpawnFromPool("test", weapon.transform.position, Quaternion.identity); // test
                //bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, weapon.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //bullet.GetComponent<Bullet>().Shoot(direction);
                break;

            case AttackType.Cast:
                break;
        }
    }
}
