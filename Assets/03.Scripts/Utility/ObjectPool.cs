using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : BaseMonoSingleton<ObjectPool<T>> where T : MonoBehaviour
{
    [SerializeField] GameObject poolingObjectPrefab;
    [SerializeField] Queue<T> poolingObjectQueue = new Queue<T>();
    
    protected void Start()
    {
        Initialize(10);
    }
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    public T CreateNewObject()
    {
        T newObj = Instantiate(poolingObjectPrefab).GetComponent<T>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public T GetObject()
    {
        if (poolingObjectQueue.Count > 0)
        {
            var obj = poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T newObj = CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.transform.position = new Vector3(0, 0, 0);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
