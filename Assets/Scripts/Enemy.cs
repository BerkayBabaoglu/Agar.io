using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public int score = 0;
    public float growFactor = 0.01f; //yedikce buyume orani
    public float growSpeed = 5f;

    private float currentScale = 0.34f;
    private Vector2 targetScale;
    private bool shouldScale = false;
    public string enemyName; // public yapıldı
    public bool isDead = false; // public yapıldı

    private void Start()
    {
        targetScale = transform.localScale;
        // Düşmanı lider tablosuna ekle
        AddEnemyToLeaderboard();
    }

    private void Update()
    {
        if (shouldScale)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * 5f);
        }
    }

    public void SetName(string newName)
    {
        enemyName = newName;
        if (nameText != null)
        {
            nameText.text = newName;
        }
        // İsim değiştiğinde lider tablosunu güncelle
        UpdateEnemyInLeaderboard();
    }

    public void AddScore(int amount)
    {
        score += amount;
        ScaleArtir();
        UpdateEnemyInLeaderboard();
    }

    void ScaleArtir()
    {
        currentScale += 0.01f;
        targetScale = new Vector3(currentScale, currentScale, 1);
        shouldScale = true;
    }

    void AddEnemyToLeaderboard()
    {
        if (string.IsNullOrEmpty(enemyName))
        {
            enemyName = "Enemy";
        }

        // Eğer düşman zaten listede varsa güncelle, yoksa ekle
        LeaderboardEntry existingEntry = ScoreManager.allEntries.Find(entry => entry.name == enemyName && !entry.isPlayer);
        if (existingEntry != null)
        {
            existingEntry.score = score;
        }
        else
        {
            ScoreManager.allEntries.Add(new LeaderboardEntry { name = enemyName, score = score, isPlayer = false });
        }
    }

    void UpdateEnemyInLeaderboard()
    {
        if (string.IsNullOrEmpty(enemyName))
        {
            return;
        }

        LeaderboardEntry enemyEntry = ScoreManager.allEntries.Find(entry => entry.name == enemyName && !entry.isPlayer);
        if (enemyEntry != null)
        {
            enemyEntry.score = score;
        }
        else
        {
            AddEnemyToLeaderboard();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy otherEnemy = collision.GetComponent<Enemy>();

        if (otherEnemy != null && otherEnemy != this)
        {
            if (score > otherEnemy.score)
            {
                AddScore(otherEnemy.score);

                float otherScale = otherEnemy.transform.localScale.x;
                currentScale += otherScale;
                targetScale = new Vector3(currentScale, currentScale, 1);
                shouldScale = true;

                // Yenilen düşmanı "Öldü" olarak işaretle
                MarkEnemyAsDead(otherEnemy);
                
                otherEnemy.gameObject.SetActive(false);
            }
        }
    }

    void MarkEnemyAsDead(Enemy deadEnemy)
    {
        // Düşmanı "Öldü" olarak işaretle
        var entry = ScoreManager.allEntries.Find(e => e.name == deadEnemy.enemyName && !e.isPlayer);
        if (entry != null)
        {
            entry.isDead = true;
        }
        
        // Ölen düşmanın isDead özelliğini de güncelle
        deadEnemy.isDead = true;
        
        // Aktif oyuncu sayısını güncelle
        UpdateActivePlayerCount();
    }

    void UpdateActivePlayerCount()
    {
        int activePlayers = 0;
        int totalPlayers = 0;
        
        foreach (var enemy in EnemyManager.allEnemies)
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
        
        // Oyuncu sayısını da ekle
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

    private void OnDestroy()
    {
        // Düşman yok edildiğinde "Öldü" olarak işaretle
        if (!string.IsNullOrEmpty(enemyName))
        {
            var entry = ScoreManager.allEntries.Find(e => e.name == enemyName && !e.isPlayer);
            if (entry != null)
            {
                entry.isDead = true;
            }
        }
    }
}