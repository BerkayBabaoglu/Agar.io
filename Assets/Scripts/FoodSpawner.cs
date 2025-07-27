using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public Vector2 spawnAreaMin = new Vector2(-17f, -13.44f);
    public Vector2 spawnAreaMax = new Vector2(17.9f, 13.35f);
    public int initialPoolSize = 100;
    public int minActiveFoods = 70;

    private List<GameObject> fooPool = new List<GameObject>();

    public Transform foodParent;


    private void Start()
    {
        CreatePool();
        SpawnInitialFoods();
    }

    private void Update()
    {
        int activeCount = 0;
        foreach (var food in foodPrefabs)
        {
            if(food.activeInHierarchy)
                activeCount++;
        }

        if(activeCount < minActiveFoods)
        {
            int needed = minActiveFoods - activeCount;
            SpawnFoods(needed);
        }
    }

    void CreatePool()
    {
        for(int i=0;i<initialPoolSize; i++)
        {
            GameObject prefab = foodPrefabs[Random.Range(0,foodPrefabs.Length)];
            GameObject food = Instantiate(prefab, Vector3.zero, Quaternion.identity,foodParent);
            food.SetActive(false);
            fooPool.Add(food);
        }
    }

    void SpawnInitialFoods()
    {
        SpawnFoods(initialPoolSize);
    }

    void SpawnFoods(int count)
    {
        int spawned = 0;

        foreach (GameObject food in fooPool) {

            if (!food.activeInHierarchy)
            {
                Vector2 spawnPos = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), 
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y));

                food.transform.position = spawnPos;
                food.SetActive(true);

                spawned++;
                if (spawned >= count)
                    break;
            }
        
        } 

    }
}
