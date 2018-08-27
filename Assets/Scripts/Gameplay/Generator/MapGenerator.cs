using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct SpecialRoom
    {
        public GameObject room;
        public int id;
    }

    public List<GameObject> listCommonRooms;
    public List<SpecialRoom> listSpecialRooms;

    public GameObject parent;
    public GameObject playerSpawnRoom;
    // Use this for initialization

    private GeneratorArrayMap.RawData[,] _data;
    private GeneratorArrayMap _generate;
    public GameObject[,] listRoom;
    public Point currentRoomPoint;

    public static MapGenerator instance;

	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SpawnPlayer(Player player)
    {
        player.transform.position = playerSpawnRoom.transform.position + playerSpawnRoom.transform.Find("EventObject").Find("PlayerSpawn").transform.localPosition;
        //Debug.Log(player.transform.position);
    }

    public void Init()
    {
        if (instance != null && instance != this)
        {
            instance = null;
        }
        instance = this;
        currentRoomPoint = new Point();
    }

    [ContextMenu("Create Map")]
    public void CreateMap()
    {
        _generate = gameObject.GetComponent<GeneratorArrayMap>();

        List<int> ids = new List<int>();
        foreach (SpecialRoom room in listSpecialRooms)
        {
            ids.Add(room.id);
        }

        _generate.GetArrayData(ids);


        Vector2 startVec = new Vector2();
        float w = 0, h = 0;
        if (listCommonRooms.Count == 0)
        {
            Debug.LogWarning("Warning, there is no one room for create");
            return;
        }

        if (listCommonRooms[0].GetComponent<SpriteRenderer>() != null)
        {
            w = listCommonRooms[0].GetComponent<SpriteRenderer>().bounds.size.x;
            h = listCommonRooms[0].GetComponent<SpriteRenderer>().bounds.size.y;
        }
        else if (listCommonRooms[0].GetComponent<Tilemap>() != null)
        {
            w = listCommonRooms[0].GetComponent<Tilemap>().size.x;
            h = listCommonRooms[0].GetComponent<Tilemap>().size.y;
        }
        else
        {
            return;
        }

        //Debug.Log(w + " " + h);
        //Debug.Log(Camera.main.pixelWidth + " " + Camera.main.pixelHeight);
        startVec.x = 0 - ((_generate.Width / 2) * w);
        startVec.y = 0 + ((_generate.Height / 2) * h);
        GameObject tempRoom;
        listRoom = new GameObject[_generate.Height, _generate.Width];

        for (int i = 0; i < _generate.Height; i++)
        {
            for (int j = 0; j < _generate.Width; j++)
            {
                if (_generate.arrayData[i, j].data != 0)
                {
                    if (_generate.arrayData[i, j].data == 1)
                    {
                        tempRoom = Instantiate(listCommonRooms[UnityEngine.Random.Range(0, listCommonRooms.Count)]);
                    }
                    else
                    {
                        //tempRoom = Instantiate(listSpecialRooms[_generate.arrayData[i, j].data].room);
                        tempRoom = SpawnSpecialRoom(_generate.arrayData[i, j].data, listSpecialRooms);
                        if (_generate.arrayData[i, j].data == 2)
                        {
                            currentRoomPoint.x = i;
                            currentRoomPoint.y = j;
                        }
                    }
                    if (tempRoom == null) continue;
                    //room = Instantiate(listCommonRooms[UnityEngine.Random.Range(0, listCommonRooms.Count)]);
                    Vector2 pos = new Vector3();
                    pos.x = startVec.x + (w * j);
                    pos.y = startVec.y - (h * i);
                    tempRoom.transform.position = pos;
                    tempRoom.transform.parent = parent.transform;
                    tempRoom.name = "Area_" + i + "_" + j + "_Type_" + _generate.arrayData[i, j].data;
                    tempRoom.GetComponent<Room>().InitRoom(i, j);
                    tempRoom.SetActive(false);
                    listRoom[i, j] = tempRoom;
                }
                else
                {
                    listRoom[i, j] = null;
                }
            }
        }

        //Debug.Log("test.................");
    }


    GameObject SpawnSpecialRoom(int id, List<SpecialRoom> listRoom)
    {
        foreach (SpecialRoom sr in listRoom)
        {
            if (sr.id == id)
            {
                //return Instantiate(sr.room);
                return SpawnSpecialRoomByIndex(sr.id, sr.room);
            }
        }
        return null;
    }

    GameObject SpawnSpecialRoomByIndex(int id, GameObject room)
    {
        //Debug.Log("id " + id);
        switch (id)
        {
            case 2:
                return SpawnStartRoom(room);
            case 3:
                return SpawnOtherSpecialRoom(room);
        }
        return null;
    }

    GameObject SpawnStartRoom(GameObject room)
    {
        playerSpawnRoom = Instantiate(room);
        return playerSpawnRoom;

    }

    GameObject SpawnOtherSpecialRoom(GameObject room)
    {
        return Instantiate(room);
    }

    [ContextMenu("Add new Special Room")]
    public void AddNewSpecialRoom()
    {
        listSpecialRooms.Add(new SpecialRoom());
    }

    void OnValidate()
    {
       
    }
}
