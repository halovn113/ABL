using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static Type GetAssemblyType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null) return type;
        }
        return null;
    }

    public static string GetFullName(GameObject go)
    {
        string name = go.name;
        while (go.transform.parent != null)
        {

            go = go.transform.parent.gameObject;
            name = go.name + "." + name;
        }
        name.Replace("(Clone)", string.Empty);
        return name;
    }
    public static List<GameObject> FindAllPrefabInstances(UnityEngine.Object myPrefab)
    {
        List<GameObject> result = new List<GameObject>();
        GameObject[] allObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (GameObject GO in allObjects)
        {
#if UNITY_EDITOR
            if (PrefabUtility.GetPrefabType(GO) == PrefabType.PrefabInstance)
            {
                if (CheckGameObjectIsParentObject(GO, (GameObject)myPrefab))
                {
                    result.Add(GO);
                }
            }
#endif
            if (Application.isPlaying && !Application.isEditor)
			{
				if (CheckGameObjectIsParentObject(GO, (GameObject)myPrefab))
				{
					result.Add(GO);
				}
			}
        }
        return result;
    }

    public static List<T> FindAllPrefabInstancesWithType<T>(UnityEngine.Object myPrefab)
    {
        List<T> result = new List<T>();
        GameObject[] allObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (GameObject GO in allObjects)
        {
#if UNITY_EDITOR
            if (PrefabUtility.GetPrefabType(GO) == PrefabType.PrefabInstance)
            {
                if (CheckGameObjectIsParentObject(GO, (GameObject)myPrefab))
                {
                    if (GO.GetComponent<T>() != null)
                    {
                        result.Add(GO.transform.GetComponent<T>());
                    }
                }
            }
#endif
            if (Application.isPlaying && !Application.isEditor)
            {
                if (CheckGameObjectIsParentObject(GO, (GameObject)myPrefab))
                {
                    if (GO.GetComponent<T>() != null)
                    {
                        result.Add(GO.transform.GetComponent<T>());
                    }
                }
            }
        }
        return result;
    }

    public static bool CheckGameObjectIsParentObject(GameObject go, GameObject parent)
    {
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            if (go == parent)
            {
                return true;
            }
        }
        return false;
    }

    public static float Angle360(Vector2 p1, Vector2 p2, Vector2 o = default(Vector2))
    {
        Vector2 v1, v2;
        if (o == default(Vector2))
        {
            v1 = p1.normalized;
            v2 = p2.normalized;
        }
        else
        {
            v1 = (p1 - o).normalized;
            v2 = (p2 - o).normalized;
        }
        float angle = Vector2.Angle(v1, v2);
        return Mathf.Sign(Vector3.Cross(v1, v2).z) < 0 ? (360 - angle) % 360 : angle;
    }
}
