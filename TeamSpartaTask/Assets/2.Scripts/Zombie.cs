using System;
using UnityEngine;


public class Zombie : MonoBehaviour
{
    [Header("Test")]
    public bool testLog = false;
    public int testId = -1;
    public float testmoveSpeed = 1.0f;
    public float testmoveBackSpeed = 3.0f;
    public float testclimbSpeed = 3.0f;
    public float testfallSpeed = 3.0f;
    public float testClimbCool = 1.0f;
    public int testHealth = 10;

    private bool isInitialized = false;

    public int zombieId { get; private set; }
    public float moveSpeed { get; private set; }
    public float moveBackSpeed { get; private set; }
    public float climbSpeed { get; private set; }
    public float fallSpeed { get; private set; }
    public float climbCoolTime { get; private set; }
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }


    private ZombieState currentState;
    private ZombieState nextState;

    public ZombieState_Attack zombieState_Attack { get; private set; }
    public ZombieState_Climb zombieState_Climb { get; private set; }
    public ZombieState_Die zombieState_Die { get; private set; }
    public ZombieState_Fall zombieState_Fall { get; private set; }
    public ZombieState_Move zombieState_Move { get; private set; }
    public ZombieState_MoveBack zombieState_MoveBack { get; private set; }

    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public Vector2 capsuleSize { get; private set; }
    public float radius { get; private set; }
    public float defaultMass { get; private set; }
    public float target { get; private set; }
    public int leftZombieId { get; private set; }
    public float lastClimbTime { get; private set; }

    public Action<GameObject> onZombieDead;

    public void Initialize(int zombieId, LayerMask layerMaskZombie, LayerMask layerMaskTerrain, Action<GameObject> onZombieDead)
    {
        this.zombieId = zombieId;

        if(isInitialized == false)
        {
            CreateState(layerMaskZombie, layerMaskTerrain);
            this.onZombieDead = onZombieDead;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            rb.excludeLayers ^= (layerMaskZombie | layerMaskTerrain);
            capsuleCollider = GetComponent<CapsuleCollider2D>();
            capsuleSize = capsuleCollider.size;
            radius = capsuleCollider.size.x * 0.5f;
            defaultMass = rb.mass;
            leftZombieId = -1;
            lastClimbTime = -1;
            isInitialized = true;
        }
    }

    public void CreateState(LayerMask layerMaskZombie, LayerMask layerMaskTerrain)
    {
        zombieState_Attack = new ZombieState_Attack(layerMaskZombie, layerMaskTerrain);
        zombieState_Climb = new ZombieState_Climb(layerMaskZombie, layerMaskTerrain);
        zombieState_Die = new ZombieState_Die(layerMaskZombie, layerMaskTerrain);
        zombieState_Fall = new ZombieState_Fall(layerMaskZombie, layerMaskTerrain);
        zombieState_Move = new ZombieState_Move(layerMaskZombie, layerMaskTerrain);
        zombieState_MoveBack = new ZombieState_MoveBack(layerMaskZombie, layerMaskTerrain);
    }

    public void ResetStatus()
    {
        if (nextState != null)
        {
            nextState.Exit(this);
        }
        nextState = null;
        currentState = zombieState_Move;
        currentState.Enter(this);

        currentHealth = maxHealth;
    }

    public void SetData(float moveSpeed, float moveBackSpeed, float climbSpeed, float fallSpeed, float climbCoolTime, int maxHealth)
    {
        this.moveSpeed = moveSpeed;
        this.moveBackSpeed = moveBackSpeed;
        this.climbSpeed = climbSpeed;
        this.fallSpeed = fallSpeed;
        this.climbCoolTime = climbCoolTime;
        this.maxHealth = maxHealth;
    }

    public void SetTarget(float target)
    {
        this.target = target;
    }

    public void SetLeftZombieId(int leftZombieId)
    {
        this.leftZombieId = leftZombieId;
    }

    public void ResetLeftZombieId()
    {
        leftZombieId = -1;
    }

    public void SetClimbTime(float currentTime)
    {
        lastClimbTime = currentTime;
    }

    public bool IsDeadByDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public void OnAttackAnimationEvent()
    {

    }

    private void ChangeState(ZombieState nextState)
    {
        currentState.Exit(this);
        currentState = nextState;
        currentState.Enter(this);
        nextState = null;
    }

    private void Start()
    {
        if(isInitialized == false)
        {
            Debug.Log("Test Init");
            Initialize(testId, LayerMask.GetMask("Zombie01"), LayerMask.GetMask("Terrain01"), null);

            moveSpeed = testmoveSpeed;
            moveBackSpeed = testmoveBackSpeed;
            climbSpeed = testclimbSpeed;
            fallSpeed = testfallSpeed;
            climbCoolTime = testClimbCool;

            ResetStatus();
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        nextState = currentState.OnCollisionEnter(this, collision);

        if(nextState != null)
        {
            ChangeState(nextState);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        nextState = currentState.OnCollisionStay(this, collision);

        if (nextState != null)
        {
            ChangeState(nextState);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        nextState = currentState.OnCollisionExit(this, collision);

        if (nextState != null)
        {
            ChangeState(nextState);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        nextState = currentState.OnTriggerStay(this, collider);

        if (nextState != null)
        {
            ChangeState(nextState);
        }
    }
}
