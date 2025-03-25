using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Data")]
    public Transform spawnPoint;
    public GameObject zombiePrefab;

    [Header("Zombie Data")]
    public LayerMask layerMaskZombie;
    public LayerMask layerMaskTerrain;
    public float moveSpeed;
    public float moveBackSpeed;
    public float climbSpeed;
    public float fallSpeed;
    public float climbCoolTime;
    public int maxHealth;

    private GameObjectPool zombieObjectPool;
    private GameObject zombieObject;
    private Zombie zombieScript;
    private int recentZombieId;

    private void Start()
    {
        recentZombieId = 0;
        zombieObjectPool = new GameObjectPool(zombiePrefab, 10, 5, transform);
    }

    public void SpawnZombie()
    {
        zombieObject = zombieObjectPool.Get();
        zombieObject.transform.position = spawnPoint.position;
        zombieObject.layer = (int)Mathf.Log(layerMaskZombie, 2f);
        zombieScript = zombieObject.GetComponent<Zombie>();
        zombieScript.Initialize(recentZombieId++, layerMaskZombie, layerMaskTerrain, ZombieDead);
        zombieScript.SetData(moveSpeed, moveBackSpeed, climbSpeed, fallSpeed, climbCoolTime, maxHealth);
        zombieScript.ResetStatus();
        Debug.Log($"{recentZombieId}th zombie spawned");
    }

    public void ZombieDead(GameObject zombieObject)
    {
        zombieObjectPool.Release(zombieObject);
    }
}
