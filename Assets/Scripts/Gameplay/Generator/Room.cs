using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum AreaType
    {
        Room,
        Area
    }

    public enum typeRoom
    {
        Type1x1,
        Type1x2,
        Type2x1,
        Type2x2,
        Free,
    }

    public typeRoom type;
    public AreaType areaType;
    public int numberEnemies;
    public int enemyLeft;

    public BoxCollider2D[] doors;
    public Point[] connectedRooms;
    public GameObject[] paths;
    public GameObject[] walls;

    [ContextMenu("GetDoors")]
    public void GetDoors()
    {
        if (areaType == AreaType.Room)
        {
            doors = transform.Find("EventObject").Find("Doors").GetComponentsInChildren<BoxCollider2D>();
        }
        else if (areaType == AreaType.Area)
        {
            
        }
    }

    /// <summary>
    /// nhập giá trị các giá trị array xung quanh nó để mở đường đi theo giá trị
    /// khác 0 sẽ mở. Mặc định 4 hướng: 0: trên, 1: dưới, 2: trái, 3: phải
    /// </summary>
    /// <param name="arrays"></param>
    public void GetPath(int[] arrays)
    {
        if (walls.Length < arrays.Length || paths.Length < arrays.Length)
        {
            Debug.LogWarning("Wall or Path length is less than array");
            return;
        }
        for (int i = 0; i < arrays.Length; i++)
        {
            if (arrays[i] != 0)
            {
                walls[i].SetActive(false);
                paths[i].SetActive(true);
            }
            else
            {
                walls[i].SetActive(true);
                paths[i].SetActive(false);
            }
        }
    }

    public void InitRoom(int x, int y)
    {
        switch (type)
        {
            case typeRoom.Type1x1:
                Init1x1(x, y);
                break;
        }
        
    }

    public void Init1x1(int x, int y)
    {
        connectedRooms = new Point[4];
        for (int i = 0; i < connectedRooms.Length; i++)
        {
            connectedRooms[i] = new Point();
        }
        connectedRooms[0].x = x - 1;
        connectedRooms[0].y = y;

        connectedRooms[1].x = x + 1;
        connectedRooms[1].y = y;

        connectedRooms[2].x = x;
        connectedRooms[2].y = y + 1;

        connectedRooms[3].x = x;
        connectedRooms[3].y = y - 1;
    }

    public void Init2x1(int x, int y)
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {   
        if (coll.gameObject.tag == "Player")
        {
            GameDirector.instance.currentRoom = this;
        }
    }
}
