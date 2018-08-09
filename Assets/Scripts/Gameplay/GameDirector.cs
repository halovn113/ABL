using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    //  -- Directors -- //
    [Header("Directors Settings")]
    public UIDirector UIDirector;


    public Player player;
    public Enemy enemyTest;
    public AI aiTest;
    public static GameDirector instance;
    void Awake()
    {
        if (instance != null)
        {
            instance = null;
        }
        instance = this;

    }
    
	// Use this for initialization
	void Start ()
    {
        //aiTest.GetComponent<Actor>().MoveToTime(player.transform.position, 2);
        //aiTest.GetComponent<Actor>().MoveTo(player.transform.position, 2);
        DOTween.Init();
        UIDirector.UIInit();
        player.HealthUpdate(-50);
        //enemyTest.FollowPlayer();
        //enemyTest.ChaseAndAttackPlayer();
    }

    // Update is called once per frame
    void Update ()
    {
        CheckUnitCondition();
        ControlAI();
	}

    void CheckUnitCondition()
    {
        CheckPlayerCondition();
    }

    void CheckPlayerCondition()
    {
        if (player.unitState == UnitState.Dead)
        {
            Debug.Log("Player is dead");
        }
    }

    void ControlAI()
    {
  
            //aiTest.MoveTo(player.transform.position);
            //aiTest.MoveToPlayer();

    }
}
