using UnityEngine;

public class ZombieState_Fall : ZombieState
{
    public ZombieState_Fall(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {

    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start fall");
        }
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop fall");
        }
    }
    public override void FixedUpdate(Zombie zombie)
    {
        rb = zombie.rb;

        finalVelocity.x = rb.velocity.x;
        finalVelocity.y = -zombie.fallSpeed;

        rb.velocity = finalVelocity;
    }

    public override ZombieState OnCollisionStay(Zombie zombie, Collision2D collision)
    {
        collisionLayer = collision.gameObject.layer;

        if (collisionLayer == layerMaskDamageable)
        {
            collisionBullet = collision.gameObject.GetComponent<Bullet>();
            collisionBullet.DestroyBullet();

            if (zombie.IsDeadByDamage(10))
            {
                return zombie.zombieState_Die;
            }
            else
            {
                return null;
            }
        }

        if (collisionLayer == layerMaskTerrain)
        {
            return zombie.zombieState_Move;
        }

        if (collisionLayer == layerMaskZombie)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            if (currentPosition.y - zombie.capsuleSize.y + zombie.radius > collisionPosition.y)
            {
                return zombie.zombieState_Move;
            }
        }

        return null;
    }
}