using UnityEngine;

public class ZombieState_Attack : ZombieState
{
    private bool isOnTerrain;

    public ZombieState_Attack(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {

    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start attack");
        }

        isOnTerrain = false;
        zombie.rb.mass = zombie.defaultMass * 500f;
        zombie.animator.SetBool("IsAttacking", true);
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop attack");
        }
        zombie.rb.mass = zombie.defaultMass;
        zombie.animator.SetBool("IsAttacking", false);
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

        if(collisionLayer == layerMaskDamageable)
        {
            if(zombie.IsDeadByDamage(10))
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
            isOnTerrain = true;
            zombie.rb.mass = zombie.defaultMass;
        }

        if (collisionLayer == layerMaskZombie)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            if (collisionPosition.y > currentPosition.y
                && collisionPosition.x < currentPosition.x + 0.01f
                && isOnTerrain)
            {
                return zombie.zombieState_MoveBack;
            }
        }

        return null;
    }

    public override ZombieState OnCollisionExit(Zombie zombie, Collision2D collision)
    {
        collisionLayer = collision.gameObject.layer;

        if (collisionLayer == layerMaskTerrain)
        {
            isOnTerrain = false;
        }

        return null;
    }
}
