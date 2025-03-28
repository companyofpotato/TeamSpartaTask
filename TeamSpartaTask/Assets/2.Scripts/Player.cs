using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Spawner Data")]
    public Transform spawnPoint;
    public GameObject bulletPrefab;

    [Header("Bullet Data")]
    public float bulletSpeed;
    public float bulletCoolTime;
    public LayerMask terrainLayerMask;

    [Header("Weapon")]
    public Transform gunTransform;

    private float spawnPositionX;
    private float spawnPositionY;
    private float gunPositionX;
    private float gunPositionY;

    private GameObjectPool bulletObjectPool;
    private GameObject bulletObject;
    private Bullet bulletScript;

    private float lastFiredTime;
    private Vector2 mousePosition;

    private void Start()
    {
        bulletObjectPool = new GameObjectPool(bulletPrefab, 10, 5, transform);
        lastFiredTime = Time.time;

        spawnPositionX = spawnPoint.position.x;
        spawnPositionY = spawnPoint.position.y;
        gunPositionX = gunTransform.position.x;
        gunPositionY = gunTransform.position.y;
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 총구가 커서 위치를 향하도록 조정
        gunTransform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(mousePosition.y - gunPositionY, mousePosition.x - gunPositionX) * Mathf.Rad2Deg) - 45f);

        if (Input.GetMouseButtonDown(0) && lastFiredTime + bulletCoolTime <= Time.time)
        {
            // 총알 생성 지점에서 마우스 커서 위치로 향하는 벡터 계산
            mousePosition.x -= spawnPositionX;
            mousePosition.y -= spawnPositionY;

            bulletObject = bulletObjectPool.Get();
            bulletObject.transform.position = spawnPoint.position;
            bulletScript = bulletObject.GetComponent<Bullet>();
            bulletScript.SetData(mousePosition.normalized, bulletSpeed, terrainLayerMask, BulletDestroy);
            lastFiredTime = Time.time;
        }
    }

    public void BulletDestroy(GameObject bulletObject)
    {
        bulletObjectPool.Release(bulletObject);
    }
}
