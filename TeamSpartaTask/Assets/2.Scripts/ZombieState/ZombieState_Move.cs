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
        rb = zombie.rb;

        finalVelocity.x = -zombie.moveSpeed;
        finalVelocity.y = rb.velocity.y;

        if (finalVelocity.y > zombie.climbSpeed)
        {
            // 너무 높게 올라가는 현상 방지
            finalVelocity.y = zombie.climbSpeed;
        }

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

        if (collisionLayer == layerMaskPlayer)
        {
            return zombie.zombieState_Attack;
        }

        if (collisionLayer == layerMaskZombie)
        {
            collisionPosition = collision.gameObject.transform.position;
            currentPosition = zombie.transform.position;

            // 이동 중 왼쪽 적당한 높이에 좀비가 닿으면
            if (collisionPosition.x < currentPosition.x
                && currentPosition.y - zombie.capsuleSize.y + zombie.radius < collisionPosition.y
                && collisionPosition.y < currentPosition.y + zombie.capsuleSize.y * 0.5f - zombie.radius) 
            {
                collisionZombie = collision.gameObject.GetComponent<Zombie>();

                // 타고 올라갈 좀비의 ID값을 기억해두고 올라간다.
                zombie.SetLeftZombieId(collisionZombie.zombieId); 
                return zombie.zombieState_Climb;
            }
        }

        return null;
    }

    // 원거리 공격 좀비를 위해 공격 판정을 trigger collider를 통해 확인(추후 구현)
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

