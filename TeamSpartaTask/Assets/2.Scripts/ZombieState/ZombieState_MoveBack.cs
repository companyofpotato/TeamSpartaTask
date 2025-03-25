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

        // 뒤로 밀릴 지점 설정
        zombie.SetTarget(zombie.transform.position.x + zombie.capsuleSize.x + 0.2f);

        // 뒤에 축적된 좀비들을 밀기 위해 질량 증가
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

        // 좀비와 닿던 지면과 닿던 필요한 지점까지 밀리면
        if (zombie.target <= zombie.transform.position.x)
        {
            // 다시 왼쪽으로 이동 시작하여 너무 많이 밀리는 현상 방지
            return zombie.zombieState_Move; 
        }

        return null;
    }
}
