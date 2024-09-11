using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    public GameObject fireballPrefab;
    public int poolSize = 10;
    private Queue<GameObject> fireballPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab);
            fireball.SetActive(false);
            fireballPool.Enqueue(fireball);
        }
    }

    public GameObject GetFireball()
    {
        if (fireballPool.Count > 0)
        {
            GameObject fireball = fireballPool.Dequeue();
            fireball.SetActive(true);
            return fireball;
        }
        else
        {
            GameObject fireball = Instantiate(fireballPrefab);
            return fireball;
        }
    }

    public void ReturnFireball(GameObject fireball)
    {
        fireball.SetActive(false);
        fireballPool.Enqueue(fireball);
    }
}