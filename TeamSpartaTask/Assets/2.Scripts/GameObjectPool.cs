using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool : MonoBehaviour
{
    private int maxCount;
    private int initCount;
    private GameObject prefab;
    private ObjectPool<GameObject> pool;
    private Transform location;

    private GameObject result;

    public GameObjectPool(GameObject prefab, int maxCount, int initCount, Transform location)
    {
        this.prefab = prefab;
        this.maxCount = maxCount;
        this.initCount = initCount;
        this.location = location;

        pool = new ObjectPool<GameObject>(CreateObject, ActivateObject, DeactivateObject, DestroyObject, false, initCount, maxCount);

        for(int idx = 0;idx < initCount;idx++)
        {
            Release(CreateObject());
        }
    }

    private GameObject CreateObject()
    {
        return Instantiate(prefab, location);
    }

    private void ActivateObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void DeactivateObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject Get()
    {
        if (pool.CountActive >= maxCount)
        {
            result = CreateObject();
            result.tag = "OverCountObject";
        }
        else
        {
            result = pool.Get();
        }

        return result;
    }

    public void Release(GameObject gameObject)
    {
        if (gameObject.CompareTag("OverCountObject"))
        {
            Destroy(gameObject);
        }
        else
        {
            pool.Release(gameObject);
        }
    }
}
