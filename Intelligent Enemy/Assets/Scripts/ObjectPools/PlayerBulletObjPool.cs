using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBulletObjPool : MonoBehaviour
{
    [SerializeField]
    GameObject objectToPool;
    [SerializeField]
    Transform spawner;
    [SerializeField]
    [Range(1, 30)]
    int maxObjects = 10;

    public static PlayerBulletObjPool instance;
    GameObject[] pooledObjects;
    GameObject tempObj;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pooledObjects = new GameObject[maxObjects];
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            tempObj = Instantiate(objectToPool);
            tempObj.SetActive(false);
            tempObj.transform.parent = transform;
            pooledObjects[i] = tempObj;
        }
    }

    GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeSelf == false)
            {
                return obj;
            }
        }
        Debug.LogError("No available player bullets");
        return null;
    }

    public GameObject SpawnPooledObject()
    {
        GameObject obj;
        if (obj = GetPooledObject())
        {
            obj.SetActive(true);
            obj.transform.position = spawner.position;
            return obj;
        }
        return null;
    }
}