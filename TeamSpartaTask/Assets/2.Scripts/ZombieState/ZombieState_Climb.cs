using UnityEngine;

public class ZombieState_Climb : ZombieState
{

    public ZombieState_Climb(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {

    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start climb");
        }
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
            // 오르는 행위에 쿨타임을 부여하여 올라가다 떨어지고 다시 오르는 행위가 과도하게 반복되는 것을 방지한다.
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
                // 우측 적당한 높이에 좀비가 있으면 떨어진다.
                return zombie.zombieState_Fall;
            }

            if (currentPosition.y + zombie.capsuleSize.y - zombie.radius < collisionPosition.y) 
            {
                // 위에 좀비가 있으면 떨어진다.
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
                // 타고 올라가던 좀비가 없어지면 왼쪽으로 이동한다.
                return zombie.zombieState_Move; 
            }
        }

        return null;
    }
}
