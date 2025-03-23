using UnityEngine;

public class ZombieState_MoveBack : ZombieState
{
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
        if (zombie.target <= zombie.transform.position.x)
        {
            return new ZombieState_Move();
        }

        return null;
    }
}
