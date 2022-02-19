using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> objectPool = new List<GameObject>();
    private List<BulletController> bulletControllers = new List<BulletController>();

    [HideInInspector]
    public bool isBulletController = false;
    private int currentIndex;

    public void PoolObjects(GameObject objectToPool, int poolAmount)
    {
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            objectPool.Add(obj);

            if(isBulletController)
            bulletControllers.Add(obj.GetComponent<BulletController>());

            obj.SetActive(false);
        }
    }

    public GameObject SpawnObj(Vector3 spawnPos, Quaternion rotation)
    {
        GameObject obj = objectPool[currentIndex];

        obj.SetActive(true);
        obj.transform.position = spawnPos;
        obj.transform.rotation = rotation;

        if (++currentIndex == objectPool.Count)
        {
            currentIndex = 0;
        }

        return obj;
    }

    public GameObject SpawnObj(Vector3 spawnPos, Quaternion rotation, Transform parent)
    {
        GameObject obj = objectPool[currentIndex];
        obj.transform.parent = parent;

        obj.SetActive(true);
        obj.transform.position = spawnPos;
        obj.transform.rotation = rotation;

        if (++currentIndex == objectPool.Count)
        {
            currentIndex = 0;
        }

        return obj;
    }

    public GameObject SpawnObj(Vector3 spawnPos, Quaternion rotation, ref BulletController bulletController)
    {
        GameObject obj = objectPool[currentIndex];
        bulletController = bulletControllers[currentIndex];

        obj.SetActive(true);
        obj.transform.position = spawnPos;
        obj.transform.rotation = rotation;

        if(++currentIndex == objectPool.Count)
        {
            currentIndex = 0;
        }

        return obj;
    }
}
