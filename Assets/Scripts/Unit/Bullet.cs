using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    enum move { shoot, nope };
    public float speed;
    Vector3 direction;
    move status = move.nope;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status == move.shoot)
        {
            BulletMove();
        }
    }

    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        direction = direction.normalized;
        status = move.shoot;
    }

    void BulletMove()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void RemoveBullet()
    {
        gameObject.SetActive(false);
    }
}
