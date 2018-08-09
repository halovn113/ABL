using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 curretPosition;

    private float tempSpeed;
    private Transform tempTarget;

    private Action action;
    private Action afterAction;

    private Queue<Action> actions;
    private bool _isActing;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (action != null)
        {
            action();
        }
	}

    public void StopAction()
    {
        action = null;
        afterAction = null;
    }

    public bool isActing()
    {
        return _isActing;
    }

    public void MoveToTime(Vector3 position, float time)
    {
        tempSpeed = (Vector3.Distance(gameObject.transform.position, position) / time);
        action = () => { MoveToInternal(position, tempSpeed); };
    }

    public void MoveTo(Vector3 position, float speed)
    {
        action = () => { MoveToInternal(position, speed); };
    }

    public void AfterActing(Action afterAction)
    {
        this.afterAction = afterAction;
    }

    /// <summary>
    /// Theo đuổi mục tiêu ở một tốc độ nhất định và dừng khi ở khoảng cách nào đó
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <param name="distance"></param>
    /// <param name="forever"></param>
    public void Follow(Transform target, float speed, float distance, bool forever)
    {
        tempTarget = target;
        action = () =>
        {
            if (Vector3.Distance(target.position, gameObject.transform.position) > distance)
            {
                _isActing = true;
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, tempTarget.transform.position, speed * Time.deltaTime);
            }
            else
            {
                if (!forever)
                {
                    //_isActing = false;
                    //action = null;
                    ActionDone();
                }
            }
        };
    }

    /// ----------------- Internal function ---------------------///
    void MoveToInternal(Vector3 position, float speed)
    {
        if (gameObject.transform.position != position)
        {
            _isActing = true;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, position, speed * Time.deltaTime);
        }
        else
        {
            tempSpeed = 0;
            //_isActing = false;
            //action = null;
            ActionDone();
        }
    }


    void ActionDone()
    {
        _isActing = false;
        action = null;
        afterAction();
    }
}
