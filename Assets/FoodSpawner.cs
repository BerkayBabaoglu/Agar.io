using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public Vector2 spawnAreaMin = new Vector2(-17f, -13.44f);
    public Vector2 spawnAreaMax = new Vector2(17.9f, 13.35f);
    public int spawnCount = 100;

    private void Start()
    {
        SpawnFoods();
    }

    void SpawnFoods()
    {
        for(int i = 0;i < spawnCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y), 0
            );

            GameObject foodPrefab = foodPrefabs[Random.Range(0, foodPrefabs.Length)];

            Instantiate(foodPrefab, spawnPos, Quaternion.identity);
        }
    }

    
}
