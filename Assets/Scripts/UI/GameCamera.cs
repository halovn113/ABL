using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D limited;

    private Vector3 _moveVector;
    private Vector3 _tempVector;

    private float _width;
    private float _height;

    public static GameCamera instance;

    void Start()
    {
        //Init();
        //instance = this;
    }

    public void Init()
    {
        _moveVector = new Vector3();
        limited = null;
        _height = GetComponent<Camera>().orthographicSize * 2f;
        _width = _height * GetComponent<Camera>().aspect;
        _tempVector = new Vector3(0, 0, 0);
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        if (limited != null)
        {
            UpdateCamera();
        }
    }

    public void UpdateArea(BoxCollider2D box)
    {
        limited = box;
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        _moveVector.x = target.transform.position.x;
        _moveVector.y = target.transform.position.y;
        _moveVector.z = transform.position.z;
        transform.position = _moveVector;

        _tempVector.x = Mathf.Clamp(transform.position.x, limited.transform.position.x - limited.size.x / 2 + _width / 2 + limited.offset.x, limited.transform.position.x + limited.size.x / 2 - _width / 2 + limited.offset.x);
        _tempVector.y = Mathf.Clamp(transform.position.y, limited.transform.position.y - limited.size.y / 2 + _height / 2 + limited.offset.y, limited.transform.position.y + limited.size.y / 2 - _height / 2 + +limited.offset.y);

        _tempVector.z = transform.position.z;
        transform.position = _tempVector;
    }
}
