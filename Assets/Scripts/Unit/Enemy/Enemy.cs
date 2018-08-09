using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : Unit
{
    public float attackDistance;
    public float moveSpeed;
    public float sightRange;
    public float nextAttack;

    public float poise;

    private EnemyActionState actionState;
    private float currentPoise;

    public void Init()
    {
        actionState = EnemyActionState.Idle;
        currentPoise = poise;
    }

	// Use this for initialization
	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch (actionState)
        {
            case EnemyActionState.Idle:
                CheckPlayer();
                break;
        }
	}

    public override void Dead()
    {
        base.Dead();
        Debug.Log("enemy dead");
        Destroy(gameObject);
    }

    public void FollowPlayer()
    {
        gameObject.GetComponent<Actor>().Follow(GameDirector.instance.player.transform, moveSpeed, attackDistance, true);
    }

    public void CheckPlayer()
    {
        if (Vector3.Distance(GameDirector.instance.player.transform.position, gameObject.transform.position) <= sightRange)
        {
            actionState = EnemyActionState.ChaseAndAttack;
            ChaseAndAttackPlayer();
        }
    }

    public void ChaseAndAttackPlayer()
    {
        gameObject.GetComponent<Actor>().Follow(GameDirector.instance.player.transform, moveSpeed, attackDistance, false);
        gameObject.GetComponent<Actor>().AfterActing(() => { Attack(); });
        
    }

    public void Attack()
    {
        Debug.Log("Attack");
        Invoke("AfterAttack", 2);
        
        //GameDirector.instance.player.HealthApply(-50);    
    }

    public void AfterAttack()
    {
        if (GameDirector.instance.player.unitState != UnitState.Dead)
        {
            ChaseAndAttackPlayer();
        }
        else
        {
            actionState = EnemyActionState.Idle;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerWeapon")
        {
            HealthUpdate(-5);
        }
    }
}

public enum EnemyActionState
{
    Idle,
    FollowPlayer,
    ChaseAndAttack,
    AfterAttack,
    RunAwayFromPlayer,
    Stun,
}
