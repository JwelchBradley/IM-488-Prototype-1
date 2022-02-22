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
    private GameObject objectToPool;

    public void PoolObjects(GameObject objectToPool, int poolAmount)
    {
        this.objectToPool = objectToPool;
        for (int i = 0; i < poolAmount; i++)
        {
            InstantiateNew(objectToPool);
        }
    }

    private void InstantiateNew(GameObject objectToPool)
    {
        GameObject obj = Instantiate(objectToPool, transform);
        objectPool.Add(obj);

        if (isBulletController)
            bulletControllers.Add(obj.GetComponent<BulletController>());

        obj.SetActive(false);
    }

    public GameObject SpawnObj(Vector3 spawnPos, Quaternion rotation)
    {
        GameObject obj = objectPool[currentIndex];

        if(obj == null)
        {
            CheckIfNullLoop(ref obj);
        }

        obj.SetActive(true);
        obj.transform.position = spawnPos;
        obj.transform.rotation = rotation;

        if (++currentIndex == objectPool.Count)
        {
            currentIndex = 0;
        }

        return obj;
    }

    private void CheckIfNullLoop(ref GameObject obj)
    {
        while(obj == null)
        {
            Debug.Log("removing null gameobject from object pool");
            objectPool.RemoveAt(currentIndex);

            if (isBulletController)
            {
                bulletControllers.RemoveAt(currentIndex);
            }

            if (++currentIndex == objectPool.Count)
            {
                currentIndex = 0;
            }

            obj = objectPool[currentIndex];
            if(obj!=null)
            Debug.Log("object name  " + obj.name);
            InstantiateNew(objectToPool);
        }
    }

    public GameObject SpawnObj(Vector3 spawnPos, Quaternion rotation, Transform parent)
    {
        GameObject obj = objectPool[currentIndex];

        if (obj == null)
        {
            CheckIfNullLoop(ref obj);
        }

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

        if (obj == null)
        {
            CheckIfNullLoop(ref obj);
        }

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
