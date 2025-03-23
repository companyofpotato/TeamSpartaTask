using System.Collections;
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

    public int zombieId { get; private set; }
    public float moveSpeed { get; private set; }
    public float moveBackSpeed { get; private set; }
    public float climbSpeed { get; private set; }
    public float fallSpeed { get; private set; }
    public float climbCoolTime { get; private set; }

    private ZombieState currentState;
    private ZombieState nextState;

    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public Vector2 capsuleSize { get; private set; }
    public float radius { get; private set; }
    public float defaultMass { get; private set; }
    public float target { get; private set; }
    public int leftZombieId { get; private set; }
    public int rightZombieId { get; private set; }
    public int upZombieId { get; private set; }
    public float lastClimbTime { get; private set; }


    public Zombie(int zombieId)
    {
        this.zombieId = zombieId;
        Initialize();
    }

    private void Initialize()
    {
        currentState = new ZombieState_Move();
        nextState = null;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        capsuleSize = capsuleCollider.size;
        radius = capsuleCollider.size.x * 0.5f;
        defaultMass = rb.mass;
        leftZombieId = -1;
        rightZombieId = -1;
        upZombieId = -1;
        lastClimbTime = -1;
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

    public void SetRightZombieId(int rightZombieId)
    {
        this.rightZombieId = rightZombieId;
    }

    public void ResetRightZombieId()
    {
        rightZombieId = -1;
    }

    public void SetUpZombieId(int upZombieId)
    {
        this.upZombieId = upZombieId;
    }

    public void ResetUpZombieId()
    {
        upZombieId = -1;
    }

    public void SetClimbTime(float currentTime)
    {
        lastClimbTime = currentTime;
    }

    private void Start()
    {
        zombieId = testId;
        moveSpeed = testmoveSpeed;
        moveBackSpeed = testmoveBackSpeed;
        climbSpeed = testclimbSpeed;
        fallSpeed = testfallSpeed;
        climbCoolTime = testClimbCool;

        Initialize();
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
            currentState.Exit(this);
            currentState = nextState;
            currentState.Enter(this);
            nextState = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        nextState = currentState.OnCollisionStay(this, collision);

        if (nextState != null)
        {
            currentState.Exit(this);
            currentState = nextState;
            currentState.Enter(this);
            nextState = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        nextState = currentState.OnCollisionExit(this, collision);

        if (nextState != null)
        {
            currentState.Exit(this);
            currentState = nextState;
            currentState.Enter(this);
            nextState = null;
        }
    }
}
