using UnityEngine;
using System.Collections.Generic;
public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Transform enemyContainer;

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin = new Vector2(-17, -13.44f);
    public Vector2 spawnAreaMax = new Vector2(17.9f, 13.35f);

    private List<string> enemyNames = new List<string>();

    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Start()
    {
        LoadNamesFromJSON();
        SpawnEnemies();
    }

    void LoadNamesFromJSON()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("EnemyNames"); //EnemyNames.jsondan cekicem
        if(jsonText != null)
        {
            NameList nameList = JsonUtility.FromJson<NameList>(jsonText.text);
            enemyNames.AddRange(nameList.names);
        }
        else
        {
            Debug.LogError("kaynaklarda EnemyNames.json yok");
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyPrefab.Length; i++) {
            Vector2 spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

            GameObject newEnemy = Instantiate(enemyPrefab[i], spawnPos, Quaternion.identity,enemyContainer);

            string assignedName = enemyNames.Count > i ? enemyNames[i] : "Enemy: " + i;
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();

            if(enemyScript != null)
            {
                enemyScript.SetName(assignedName);
            }
            else
            {
                Debug.LogWarning($"Enemy prefab at index {i} is missing the Enemy script.");
            }

            EnemyManager.allEnemies.Add(enemyScript);
        }
        
        // Düşmanlar spawn edildikten sonra aktif oyuncu sayısını güncelle
        UpdateActivePlayerCount();
    }
    
    void UpdateActivePlayerCount()
    {
        int activePlayers = 0;
        int totalPlayers = 0;
        
        // Düşmanları say
        foreach (var enemy in allEnemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                totalPlayers++;
                if (!enemy.isDead)
                {
                    activePlayers++;
                }
            }
        }
        
        // Oyuncuyu da ekle
        totalPlayers++;
        if (ScoreManager.Instance != null)
        {
            var playerEntry = ScoreManager.allEntries.Find(e => e.name == ScoreManager.Instance.playerName && e.isPlayer);
            if (playerEntry != null && !playerEntry.isDead)
            {
                activePlayers++;
            }
        }
        
        ScoreManager.UpdateGameState(true, activePlayers, totalPlayers);
    }
}

[System.Serializable]
public class NameList
{
    public string[] names;
}
