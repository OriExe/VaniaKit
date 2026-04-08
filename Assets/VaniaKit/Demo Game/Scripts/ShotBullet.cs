using System;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bullet;

    private void Start()
    {
        bullet.linearVelocity = -transform.right * 5;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject,0.07f);
    }
}
