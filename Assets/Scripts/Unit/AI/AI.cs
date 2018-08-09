using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float distance;
    public AIState aiState;
    public Transform target;
    public float moveSpeed = 1;
    float step;

    public Action<Vector3> MoveTo;
	// Use this for initialization
	void Start ()
    {
	    MoveTo = (target) =>
        {
            //while (Vector3.Distance(gameObject.transform.position, target) > 5)
            //{
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, Time.deltaTime * moveSpeed);
            //}
        };
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (aiState == AIState.Chase)
        //      {

        //      }

        step = moveSpeed * Time.deltaTime;

    }

    public void Chase(Vector3 target)
    {
        while (Vector3.Distance(gameObject.transform.position, target) > 1)
        {
            MoveTo(target);
        }
    }

    public void MoveToPlayer()
    {
            //while (Vector3.Distance(gameObject.transform.position, target) > 5)
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, GameDirector.instance.player.transform.position, Time.deltaTime * moveSpeed);
    }

    public void Move()
    {

    }
}

public enum AIState
{
    Move, 
    Chase,
    Idle,

} 
