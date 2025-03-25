using UnityEngine;

public class ZombieState_Die : ZombieState
{
    public ZombieState_Die(LayerMask layerMaskZombie, LayerMask layerMaskTerrain) : base(layerMaskZombie, layerMaskTerrain)
    {

    }

    public override void Enter(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " start die");
        }

        zombie.animator.SetBool("IsDead", true);
        zombie.onZombieDead?.Invoke(zombie.gameObject);
    }

    public override void Exit(Zombie zombie)
    {
        if (zombie.testLog)
        {
            Debug.Log(zombie.zombieId + " stop die");
        }

        zombie.animator.SetBool("IsDead", false);
    }
}
