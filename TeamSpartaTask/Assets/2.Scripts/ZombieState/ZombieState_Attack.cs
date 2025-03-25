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

        // 아래로 떨어지는 중에 사이에 끼는 것을 방지하기 위해 질량을 높인다.
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
            collisionBullet = collision.gameObject.GetComponent<Bullet>();
            collisionBullet.DestroyBullet();

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

            // 지면 위에 있을 때는 떨어질 일이 없으므로 질량을 원래대로 돌린다.
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
                // 위에 다른 좀비가 있으면 뒤로 밀린다.
                return zombie.zombieState_MoveBack; 
            }
        }

        return null;
    }
}
