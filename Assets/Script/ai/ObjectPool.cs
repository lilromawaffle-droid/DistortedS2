using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pool — recycle GameObject agar tidak Instantiate/Destroy terus.
/// Kunci performa untuk ratusan musuh sekaligus.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string      tag;
        public GameObject  prefab;
        public int         initialSize = 30;
    }

    [Header("Pools")]
    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDict;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < pool.initialSize; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDict[pool.tag] = queue;
        }
    }

    public GameObject Get(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool '{tag}' tidak ditemukan!");
            return null;
        }

        var queue = poolDict[tag];
        GameObject obj;

        if (queue.Count == 0)
        {
            // Auto-expand pool jika habis
            var pool = pools.Find(p => p.tag == tag);
            obj = pool != null ? Instantiate(pool.prefab, transform) : null;
            if (obj == null) return null;
        }
        else
        {
            obj = queue.Dequeue();
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Return(string tag, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        if (!poolDict.ContainsKey(tag))
            poolDict[tag] = new Queue<GameObject>();

        poolDict[tag].Enqueue(obj);
    }
}
