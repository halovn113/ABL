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
    public int numberEnemies;
    public int enemyLeft;

    public BoxCollider2D[] doors;
    public Point[] connectedRooms;

    [ContextMenu("GetDoors")]
    public void GetDoors()
    {
        doors = transform.Find("EventObject").Find("Doors").GetComponentsInChildren<BoxCollider2D>();
    }

    public void GetPath()
    {

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
