using UnityEngine;

public class ZombieState_Move : ZombieState
{
    public ZombieState_Move(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {
        
    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start move");
        }
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop move");
        }
    }

    public override void FixedUpdate(Zombie zombie)
    {
        Rigidbody2D rb = zombie.rb;

        finalVelocity.x = -zombie.moveSpeed;
        finalVelocity.y = rb.velocity.y;

        if (finalVelocity.y > zombie.climbSpeed)
        {
            finalVelocity.y = zombie.climbSpeed;
        }

        rb.velocity = finalVelocity;
    }

    public override ZombieState OnCollisionStay(Zombie zombie, Collision2D collision)
    {
        collisionLayer = collision.gameObject.layer;

        if (collisionLayer == layerMaskDamageable)
        {
            if (zombie.IsDeadByDamage(10))
            {
                return zombie.zombieState_Die;
            }
            else
            {
                return null;
            }
        }

        if (collisionLayer == layerMaskPlayer)
        {
            return zombie.zombieState_Attack;
        }

        if (collisionLayer == layerMaskZombie)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            if (collisionPosition.x < currentPosition.x
                && currentPosition.y - zombie.capsuleSize.y + zombie.radius < collisionPosition.y
                && collisionPosition.y < currentPosition.y + zombie.capsuleSize.y * 0.5f - zombie.radius)
            {
                collisionZombie = collision.gameObject.GetComponent<Zombie>();
                zombie.zombieState_Climb.SetLeftZombieId(collisionZombie.zombieId);
                return zombie.zombieState_Climb;
            }
        }

        return null;
    }

    //public override ZombieState OnTriggerStay(Zombie zombie, Collider2D collider)
    //{
    //    collisionLayer = collider.gameObject.layer;

    //    if (collisionLayer == layerMaskPlayer)
    //    {
    //        return zombie.zombieState_Attack;
    //    }

    //    return null;
    //}
}

