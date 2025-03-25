using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float moveSpeed;
    private Rigidbody2D rb;
    private LayerMask terrainLayerMask;
    private Action<GameObject> onBulletDestroy;

    public void SetData(Vector2 direction, float moveSpeed, LayerMask terrainLayerMask, Action<GameObject> onBulletDestroy)
    {
        this.direction = direction;
        this.moveSpeed = moveSpeed;
        this.terrainLayerMask = terrainLayerMask;
        this.onBulletDestroy = onBulletDestroy;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        rb.velocity = direction * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & terrainLayerMask) != 0)
        {
            DestroyBullet(); // 지면에 닿으면 스스로 파괴
        }
    }

    public void DestroyBullet()
    {
        onBulletDestroy?.Invoke(gameObject);
    }
}
