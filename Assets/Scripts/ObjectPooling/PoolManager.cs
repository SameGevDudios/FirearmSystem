// Object pool system by Brackeys: https://www.youtube.com/watch?v=tdSmKaJvCoA

using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject ObjectToPool;
        public int Size;
    }


    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDistionary;

    #region Singleton
    public static PoolManager Instance;
    private void Awake() => Instance = this;
    #endregion

    private void Start()
    {
        PoolDistionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject buffer = Instantiate(pool.ObjectToPool, transform);
                buffer.SetActive(false);
                objectPool.Enqueue(buffer);
            }

            PoolDistionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject InstantiateFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDistionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with " + tag + " tag doesn't exist");
            return null;
        }

        GameObject objectToSpawn = PoolDistionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject poolInterface = objectToSpawn.GetComponent<IPooledObject>();
        if (poolInterface != null)
        {
            poolInterface.OnObjectInstantiated();
        }
        PoolDistionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
