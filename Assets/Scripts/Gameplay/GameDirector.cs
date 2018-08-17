using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDirector : MonoBehaviour
{
    //  -- Directors -- //
    [Header("Directors Settings")]
    public UIDirector UIDirector;

    public GameObject CinematicCam;
    public Player player;
    public Enemy enemyTest;
    public AI aiTest;
    public static GameDirector instance;

    public Camera camera;

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
        camera = Camera.main;
        DOTween.Init();
        UIDirector.UIInit();
        player.HealthUpdate(-50);
        //enemyTest.FollowPlayer();
        //enemyTest.ChaseAndAttackPlayer();
        MapGenerator.instance.CreateMap();
        MapGenerator.instance.SpawnPlayer(player);
        //CinematicCam.GetComponent<>()
        CinematicCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        UpdateCamera(MapGenerator.instance.playerSpawnRoom);
        //UpdateCamera(player.gameObject);
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

    public void UpdateCamera(GameObject area)
    {
        CinematicCam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = area.GetComponent<CompositeCollider2D>();
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

    }
}
