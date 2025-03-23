using UnityEngine;

public class ZombieState_Move : ZombieState
{
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

        if (((1 << collisionLayer) & layerMaskPlayer) != 0)
        {
            return new ZombieState_Attack();
        }

        if (((1 << collisionLayer) & layerMaskEnemy) != 0)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            if (collisionPosition.x < currentPosition.x
                && currentPosition.y - zombie.capsuleSize.y + zombie.radius < collisionPosition.y
                && collisionPosition.y < currentPosition.y + zombie.capsuleSize.y * 0.5f - zombie.radius)
            {
                collisionZombie = collision.gameObject.GetComponent<Zombie>();
                return new ZombieState_Climb(collisionZombie.zombieId);
            }
        }

        return null;
    }
}

