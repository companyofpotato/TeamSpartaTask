using UnityEngine;

public class ZombieState_Fall : ZombieState
{
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

        if (((1 << collisionLayer) & layerMaskTerrain) != 0)
        {
            return new ZombieState_Move();
        }

        if (((1 << collisionLayer) & layerMaskEnemy) != 0)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            if (currentPosition.y - zombie.capsuleSize.y + zombie.radius > collisionPosition.y)
            {
                return new ZombieState_Move();
            }
        }

        return null;
    }
}