using UnityEngine;

public class ZombieState_Attack : ZombieState
{
    private bool isOnTerrain;

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start attack");
        }

        isOnTerrain = false;
        zombie.rb.mass = zombie.defaultMass * 500f;
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop attack");
        }
        zombie.rb.mass = zombie.defaultMass;
    }

    public override void FixedUpdate(Zombie zombie)
    {
        Rigidbody2D rb = zombie.rb;

        finalVelocity.x = 0;
        finalVelocity.y = rb.velocity.y;

        rb.velocity = finalVelocity;
    }

    public override ZombieState OnCollisionStay(Zombie zombie, Collision2D collision)
    {
        collisionLayer = collision.gameObject.layer;

        if (((1 << collisionLayer) & layerMaskTerrain) != 0)
        {
            isOnTerrain = true;
            zombie.rb.mass = zombie.defaultMass;
        }

        if (((1 << collisionLayer) & layerMaskEnemy) != 0)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;
            if (collisionPosition.y > currentPosition.y
                && collisionPosition.x < currentPosition.x + 0.01f
                && isOnTerrain)
            {
                return new ZombieState_MoveBack();
            }
        }

        return null;
    }

    public override ZombieState OnCollisionExit(Zombie zombie, Collision2D collision)
    {
        collisionLayer = collision.gameObject.layer;

        if (((1 << collisionLayer) & layerMaskTerrain) != 0)
        {
            isOnTerrain = false;
        }

        return null;
    }
}
