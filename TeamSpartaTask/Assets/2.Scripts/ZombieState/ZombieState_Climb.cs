using UnityEngine;

public class ZombieState_Climb : ZombieState
{
    private int leftZombieId;

    public ZombieState_Climb(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {

    }

    public void SetLeftZombieId(int id)
    {
        leftZombieId = id;
    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start climb");
        }

        zombie.SetLeftZombieId(leftZombieId);
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop climb");
        }

        zombie.ResetLeftZombieId();
        zombie.SetClimbTime(Time.time);
    }

    public override void FixedUpdate(Zombie zombie)
    {
        if (Time.time <= zombie.lastClimbTime + zombie.climbCoolTime)
        {
            return;
        }

        rb = zombie.rb;

        finalVelocity.x = -zombie.moveSpeed;
        finalVelocity.y = zombie.climbSpeed;

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

        if (collisionLayer == layerMaskZombie)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            if (collisionPosition.x > currentPosition.x
                && currentPosition.y - zombie.capsuleSize.y * 0.5f < collisionPosition.y)
            {
                return zombie.zombieState_Fall;
            }

            if (currentPosition.y + zombie.capsuleSize.y - zombie.radius < collisionPosition.y)
            {
                return zombie.zombieState_Fall;
            }
        }
        return null;
    }

    public override ZombieState OnCollisionExit(Zombie zombie, Collision2D collision)
    {
        collisionLayer = collision.gameObject.layer;

        if (collisionLayer == layerMaskZombie)
        {
            collisionZombie = collision.gameObject.GetComponent<Zombie>();

            if (zombie.leftZombieId == collisionZombie.zombieId)
            {
                return zombie.zombieState_Move;
            }
        }

        return null;
    }
}
