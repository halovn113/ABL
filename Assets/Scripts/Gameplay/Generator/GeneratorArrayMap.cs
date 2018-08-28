using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

// class vẫn còn trong giai đoạn test và dần hoàn thiện -- WIP
public class GeneratorArrayMap : MonoBehaviour
{
    public int Doors;
    public int Width;
    public int Height;
    public int Limited;

    public List<GameObject> listCommonRooms;
    public List<GameObject> roomObject1x2;
    public List<GameObject> roomObject2x2;
    public GameObject parentTest;
    public RawData focusPoint;

    [Serializable]
    public struct RawData
    {
        public int x;
        public int y;
        [HideInInspector]
        public int data;
    }

    public enum Option
    {
        Force4Direction = 1,
        CenterDirection = 2,
        Random4Direction = 4,
        Limited = 8,
        HasEndPoint = 16,
        FreeNumberDoors = 32,
    }

    public int count = 4; // test

    [EnumFlag]
    public Option option;

    [HideInInspector]
    public RawData[,] arrayData;

    public bool hasEndPoint;
    private int currentNumberRooms;


    [ContextMenu("Test show array")]
    public void PrintArray()
    {
        CreateEmptyArray();
        //CreateArray(arrayData);
        //for (int i = 0; i < Height; i++)
        //{
        //    var sb = new System.Text.StringBuilder();   
        //    for (int j = 0; j < Width; j++)
        //    {
        //        sb.Append("   " + arrayData[i, j].data.ToString());
        //    }
        //    //Debug.Log(sb.ToString());
        //}
        
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

        startVec.x = 0 - ((Width / 2) * w);
        startVec.y = 0 + ((Height / 2) * h);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (arrayData[i, j].data == 1)
                {
                    GameObject go = Instantiate(listCommonRooms[0]);
                    Vector2 pos = new Vector3();
                    pos.x = startVec.x + (w * j);
                    pos.y = startVec.y - (h * i);
                    go.transform.position = pos;
                    go.transform.parent = parentTest.transform;
                    go.name = "Area_" + i + "_" + j;
                }

            }
        }

        Debug.Log("test.................");
    }

    public RawData[,] GetArrayData(List<int> ids)
    {
        CreateEmptyArray();
        CreateArray(arrayData, ids);

        return arrayData;
    }

    [ContextMenu("Test_DestroyChild")]
    public void DestroyChild()
    {
        if (parentTest.transform.childCount > 0)
        {
            foreach (Transform child in parentTest.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    void CreateEmptyArray()
    {
        arrayData = new RawData[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                arrayData[i, j].data = 0;
                arrayData[i, j].x = i;
                arrayData[i, j].y = j;
            }
        }
    }

    void CreateArray(RawData[,] arrayData, List<int> ids)
    {
        RawData focus = new RawData();
        RawData end = new RawData();
        Point eP = new Point();
        Point fP = new Point();

        currentNumberRooms = 0;

        #region
        // Action work
        //Action LeftToPoint = () => { arrayData = PointToPoint(focus, arrayData[UnityEngine.Random.Range(0, Height), focus.x - 1 <= 0 ? 0 : UnityEngine.Random.Range(0, focus.x - 1)], arrayData, 1); }; // left
        //Action RightToPoint = () => { arrayData = PointToPoint(focus, arrayData[UnityEngine.Random.Range(0, Height), focus.x + 1 >= Width - 1 ? Width - 1 : UnityEngine.Random.Range(focus.x + 1, Width)],  arrayData, 1); }; // right
        //Action UpToPoint = () => { arrayData = PointToPoint(focus, arrayData[focus.y - 1 <= 0 ? 0 : UnityEngine.Random.Range(0, focus.y - 1), UnityEngine.Random.Range(0, Width)], arrayData, 1); }; // up
        //Action DownToPoint = () => { arrayData = PointToPoint(focus, arrayData[focus.y + 1 >= Height - 1 ? Height - 1 : UnityEngine.Random.Range(focus.y + 1, Height), UnityEngine.Random.Range(0, Width)], arrayData, 1); }; // down

        Action LeftToPoint = () => { arrayData = PointToPoint(focus, arrayData[UnityEngine.Random.Range(0, Height), focus.x - 1 <= 0 ? 0 : UnityEngine.Random.Range(0, focus.x - 1)], arrayData); }; // left
        Action RightToPoint = () => { arrayData = PointToPoint(focus, arrayData[UnityEngine.Random.Range(0, Height), focus.x + 1 >= Width - 1 ? Width - 1 : UnityEngine.Random.Range(focus.x + 1, Width)], arrayData); }; // right
        Action UpToPoint = () => { arrayData = PointToPoint(focus, arrayData[focus.y - 1 <= 0 ? 0 : UnityEngine.Random.Range(0, focus.y - 1), UnityEngine.Random.Range(0, Width)], arrayData); }; // up
        Action DownToPoint = () => { arrayData = PointToPoint(focus, arrayData[focus.y + 1 >= Height - 1 ? Height - 1 : UnityEngine.Random.Range(focus.y + 1, Height), UnityEngine.Random.Range(0, Width)], arrayData); }; // down

        Action FocusToPoint = () => { };
        //if (hasEndPoint)
        //{
        //     FocusToPoint = () => { arrayData = PointToPoint(focus, end, arrayData); };
        //}

        if ((option & Option.HasEndPoint) != 0)
        {
            FocusToPoint = () => { arrayData = PointToPoint(focus, end, arrayData, 1); };
        }

        #endregion

        // force center
        // bắt buộc điểm đích là trung tâm
        if ((option & Option.CenterDirection) != 0)
        {
            fP.x = Height / 2;
            fP.y = Width / 2;
        }
        else
        {
            fP.x = UnityEngine.Random.Range(0, Height);
            fP.y = UnityEngine.Random.Range(0, Width);
        }

        if ((option & Option.HasEndPoint) != 0)
        {
            eP.x = UnityEngine.Random.Range(0, Height);
            eP.y = UnityEngine.Random.Range(0, Width);
            //arrayData[eP.x, eP.y].data = 1;
        }

        //if (hasEndPoint)
        //{
        //    eP.x = UnityEngine.Random.Range(0, Height);
        //    eP.y = UnityEngine.Random.Range(0, Width);
        //}


        //arrayData[fP.x, fP.y].data = 1;
        focus = arrayData[fP.x, fP.y];
        end = arrayData[eP.x, eP.y];

        // force 4 basic directions
        // ở đây sẽ ép buộc array luôn có 4 hướng, không thể thiếu một được
        if ((option & Option.Force4Direction) != 0)
        {
            LeftToPoint();
            RightToPoint();
            UpToPoint();
            DownToPoint();
            //if (hasEndPoint)
            //{
            //    FocusToPoint();
            //}
            if ((option & Option.HasEndPoint) != 0)
            {
                FocusToPoint();
            }
        }

        // queue 4 basic direction
        // phần này thì ngược lại, dùng cho trường hợp không bắt buộc phải có 4 hướng cơ bản
        // ví dụ sử dụng script này trong trường hợp chỉ sử dụng ít hơn 4 door và ngẫu nhiên ít hơn 4 cửa không đoán trước được 

        // random 4 basic direction
        // sử dụng để tạo các cửa ngẫu nhiên nhưng vẫn thuộc 4 hướng
        // có thể có các cửa trùng hướng
        if ((option & Option.Random4Direction) != 0)
        {
            Queue<Action> queue = new Queue<Action>();
            List<Action> action = new List<Action>();

            action.Add(LeftToPoint);
            action.Add(RightToPoint);
            action.Add(UpToPoint);
            action.Add(DownToPoint);

            for (int j = 0; j < count; j++)
            {
                int i = UnityEngine.Random.Range(0, action.Count);
                queue.Enqueue(action[i]);
            }

            foreach (Action ac in queue)
            {
                ac.Invoke();
            }

            //if (hasEndPoint)
            //{
            //    FocusToPoint();
            //}
            if ((option & Option.HasEndPoint) != 0)
            {
                FocusToPoint();
            }
        }

        int numID = -1;
        //Doors = ids.Count;

        if ((option & Option.FreeNumberDoors) != 0)
        {
            List<RawData> listPoint = new List<RawData>();
            RawData temp = new RawData();
            int index = 0;

            while (index < Doors)
            {
                temp.x = UnityEngine.Random.Range(0, Height);
                temp.y = UnityEngine.Random.Range(0, Width);

                numID = UnityEngine.Random.Range(0, ids.Count);

                if (CheckIfPointIntList(temp, listPoint))
                {
                    if (ids.Count > 0)
                    {
                        PointToPoint(focus, temp, arrayData, ids[numID]);
                        ids.RemoveAt(numID);
                    }
                    else
                    {
                        //PointToPoint(focus, temp, arrayData, 1);
                        PointToPoint(focus, temp, arrayData);
                    }
                    listPoint.Add(temp);
                    index++;
                }
            }
            if ((option & Option.HasEndPoint) != 0)
            {
                FocusToPoint();
            }
        }

        //string sb = "";
        //for (int i = 0; i < arrayData.GetLength(0); i++)
        //{
        //    for (int j = 0; j < arrayData.GetLength(1); j++)
        //    {
        //        sb += "  " + arrayData[i, j].data;
        //    }
        //    sb += "\n";
        //}
        //Debug.Log(sb);
    }

    bool CheckIfPointIntList(RawData point, List<RawData> list)
    {
        if (list.Count < 1) return true;
        for (int i = 0; i < list.Count; i++)
        {
            if (point.x == list[i].x && point.y == list[i].y)
            {
                return false;
            }
        }
        return true;
    }

    //[System.Runtime.InteropServices.Optional] 
    RawData[,] PointToPoint(RawData start, RawData end, RawData[,] data, [System.Runtime.InteropServices.Optional]  int valueForDoor, [System.Runtime.InteropServices.Optional] Nullable<bool> isHorizontal)
    {
        //Debug.Log("start  x " + start.x + " , y " + start.y + ", end x" + end.x + ", y"+ end.y + " " + valueForDoor);
        if (data == null || data.Length == 0)
        {
            Debug.LogWarning("Warning, data is null or doesn't have any thing in there");
            return null;
        }
        RawData[,] d = data;
        //bool horizontal = isHorizontal.HasValue? (bool)isHorizontal : (UnityEngine.Random.Range(0, 10) % 2 == 0 ? true : false);
        bool horizontal = UnityEngine.Random.Range(0, 11) % 2 == 0 ? true : false;
        int i = start.x;
        int j = start.y;
        int goIn;
        Action hoAc = () =>
        {
            goIn = start.y < end.y ? 1 : -1;
            while (j != end.y)
            {
                if ((option & Option.Limited) != 0 && Limited != 0)
                {
                    if (currentNumberRooms == Limited)
                    {
                        return;
                    }
                    if (d[i, j].data == 0)
                    {
                        currentNumberRooms++;
                    }
                }
                if (d[i, j].data == 0)
                {
                    d[i, j].data = 1;
                }
                j += goIn;
            }
            //d[i, j].data = valueForDoor;
        };

        Action verAc = () =>
        {
            goIn = start.x < end.x ? 1 : -1;
            while (i != end.x)
            {
                if ((option & Option.Limited) != 0 && Limited != 0)
                {
                    if (currentNumberRooms == Limited)
                    {
                        return;
                    }
                    if (d[i, j].data == 0)
                    {
                        currentNumberRooms++;
                    }
                }
                if (d[i, j].data == 0)
                {
                    d[i, j].data = 1;
                }
                i += goIn;           
            }
            //d[i, j].data = valueForDoor;
        };
        if (horizontal)
        {
            hoAc();
            verAc();
            //Debug.Log("horizontal");
        }
        else
        {
            verAc();
            hoAc();
            //Debug.Log("vertical");
        }
        d[i, j].data = valueForDoor == 0 ? 1 : valueForDoor;
        return d;
    }


}
