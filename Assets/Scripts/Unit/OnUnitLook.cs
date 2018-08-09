using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUnitLook : MonoBehaviour
{
    public GameObject rootRotation;
    public GameObject[] aimThings;

    private Vector3 target;

    void Update()
    {
        //RotationUpdate();
    }

    public void RotationUpdate(Vector3 target)
    {
        if (rootRotation != null && target != null)
        {
            rootRotation.transform.rotation = Quaternion.LookRotation(Vector3.forward, rootRotation.transform.position - target);
        }
    }

    public void SetRotationObject(GameObject root, params GameObject[] aimthings)
    {
        if (root == null)
        {
            Debug.LogWarning("Warning, root rotation is null");
            return;
        }
        rootRotation = root;
        if (aimthings.Length == 0)
        {
            Debug.LogWarning("Warning, nothing in aimthing");
            return;
        }
        aimThings = aimthings;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}
