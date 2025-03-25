using UnityEngine;

public class ZombieState_MoveBack : ZombieState
{
    public ZombieState_MoveBack(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {

    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start moveBack");
        }

        zombie.SetTarget(zombie.transform.position.x + zombie.capsuleSize.x + 0.2f);
        zombie.rb.mass = zombie.defaultMass * 100f;
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop moveBack");
        }

        zombie.rb.mass = zombie.defaultMass;
    }

    public override void FixedUpdate(Zombie zombie)
    {
        rb = zombie.rb;

        finalVelocity.x = zombie.moveBackSpeed;
        finalVelocity.y = rb.velocity.y;

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

        if (zombie.target <= zombie.transform.position.x)
        {
            return zombie.zombieState_Move;
        }

        return null;
    }
}
