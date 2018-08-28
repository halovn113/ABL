using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public GameObject[,] listRoom;
    public Room currentRoom;
    public GameCamera cam;

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
        DOTween.Init();
        UIDirector.UIInit();
        MapGenerator.instance.CreateMap();
        MapGenerator.instance.SpawnPlayer(player);

        listRoom = MapGenerator.instance.listRoom;
        UpdateRenderRooms(MapGenerator.instance.currentRoomPoint.x, MapGenerator.instance.currentRoomPoint.y, true);

        cam.Init();
        cam.UpdateArea(MapGenerator.instance.playerSpawnRoom.GetComponent<BoxCollider2D>());
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
        Debug.Log(area.GetComponent<BoxCollider2D>().size.x * 64 + "  " + area.GetComponent<BoxCollider2D>().size.y);
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
    public void UpdateRenderRooms(int x, int y, bool setActive)
    {
        listRoom[x, y].SetActive(setActive);
    }

    public void UpdateRenderRoomAround(int x, int y, bool setActive)
    {
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i < 0 || y < 0 || i > listRoom.GetLength(0) - 1 || j > listRoom.GetLength(1) - 1) continue;
                if (listRoom[i, j] == null) continue;
                listRoom[i, j].SetActive(setActive);
                Debug.Log(listRoom[i,j].GetComponent<BoxCollider2D>().bounds.size.x);
            }
        }
    }
}
