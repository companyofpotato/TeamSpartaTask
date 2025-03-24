using UnityEngine;

public class ZombieState
{
    protected int layerMaskZombie;
    protected int layerMaskPlayer;
    protected int layerMaskTerrain;
    protected int layerMaskDamageable;
    protected Vector2 finalVelocity;
    protected int collisionLayer;
    protected Vector2 collisionPosition;
    protected Vector2 currentPosition;
    protected Zombie collisionZombie;
    protected Rigidbody2D rb;

    public ZombieState(LayerMask layerMaskZombie, LayerMask layerMaskTerrain)
    {
        this.layerMaskZombie = (int)Mathf.Log(layerMaskZombie, 2f);
        layerMaskPlayer = (int)Mathf.Log(LayerMask.GetMask("Player"), 2f); ;
        this.layerMaskTerrain = (int)Mathf.Log(layerMaskTerrain, 2f);
        layerMaskDamageable = (int)Mathf.Log(LayerMask.GetMask("Damageable"), 2f);

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

    public virtual ZombieState OnTriggerStay(Zombie zombie, Collider2D collider) { return null; }

}