using UnityEditor;
using UnityEngine;

public class ZombieState
{
    protected int layerMaskEnemy;
    protected int layerMaskPlayer;
    protected int layerMaskTerrain;
    protected Vector2 finalVelocity;
    protected int collisionLayer;
    protected Vector2 collisionPosition;
    protected Vector2 currentPosition;
    protected Zombie collisionZombie;
    protected Rigidbody2D rb;

    public ZombieState()
    {
        layerMaskEnemy = LayerMask.GetMask("Enemy");
        layerMaskPlayer = LayerMask.GetMask("Player");
        layerMaskTerrain = LayerMask.GetMask("Terrain");
        finalVelocity = Vector2.zero;
        collisionLayer = -1;
        collisionPosition = Vector2.zero;
        currentPosition = Vector2.zero;
        collisionZombie = null;
        rb = null;
    }

    public virtual void Enter(Zombie zombie) { }

    public virtual void Exit(Zombie zombie) { }

    public virtual void FixedUpdate(Zombie zombie) { }

    public virtual ZombieState OnCollisionEnter(Zombie zombie, Collision2D collision) { return null; }

    public virtual ZombieState OnCollisionStay(Zombie zombie, Collision2D collision) { return null; }

    public virtual ZombieState OnCollisionExit(Zombie zombie, Collision2D collision) { return null; }

}