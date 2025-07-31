using UnityEngine;
using TMPro;
using UnityEditor.iOS.Extensions.Common;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public int score = 0;
    public string playerName = "Player";
    public bool isDead = false; // Oyuncunun ölüp ölmediğini takip etmek için

    // Lider tablosu için tüm oyuncuları ve düşmanları tutacak liste
    public static List<LeaderboardEntry> allEntries = new List<LeaderboardEntry>();
    
    // Oyun durumu takibi
    public static bool isGameActive = false;
    public static int activePlayerCount = 0;
    public static int totalPlayerCount = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        UpdateScoreText();
        // Oyuncuyu lider tablosuna ekle
        AddPlayerToLeaderboard();
    }

    public void SetPlayerName(string name)
    {
        // Eski oyuncu girişini temizle
        allEntries.RemoveAll(entry => entry.name == playerName && entry.isPlayer);
        
        playerName = name;
        UpdatePlayerInLeaderboard();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        UpdatePlayerInLeaderboard();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Puan : " + score;
    }

    void AddPlayerToLeaderboard()
    {
        // Oyuncu zaten varsa güncelle, yoksa ekle
        LeaderboardEntry existingEntry = allEntries.Find(entry => entry.name == playerName && entry.isPlayer);
        if (existingEntry != null)
        {
            existingEntry.score = score;
        }
        else
        {
            allEntries.Add(new LeaderboardEntry { name = playerName, score = score, isPlayer = true });
        }
    }

    void UpdatePlayerInLeaderboard()
    {
        // Oyuncu girişini bul ve güncelle
        LeaderboardEntry playerEntry = allEntries.Find(entry => entry.name == playerName && entry.isPlayer);
        if (playerEntry != null)
        {
            playerEntry.score = score;
        }
        else
        {
            AddPlayerToLeaderboard();
        }
    }

    public static List<LeaderboardEntry> GetSortedLeaderboard()
    {
        return allEntries.OrderByDescending(entry => entry.score).ToList();
    }

    public static int GetPlayerRank()
    {
        if (Instance == null) return -1;
        
        var sortedList = GetSortedLeaderboard();
        
        for (int i = 0; i < sortedList.Count; i++)
        {
            var entry = sortedList[i];
            if (entry.name == Instance.playerName && entry.isPlayer)
            {
                return i + 1; // 1'den başlayan sıra
            }
        }
        
        return -1; // Oyuncu bulunamadı
    }

    // Debug için lider tablosunu temizleme metodu
    public static void ClearDuplicatePlayers()
    {
        var playerEntries = allEntries.Where(entry => entry.isPlayer).ToList();
        
        if (playerEntries.Count > 1)
        {
            // En yüksek puanlı oyuncu girişini tut, diğerlerini sil
            var bestPlayer = playerEntries.OrderByDescending(entry => entry.score).First();
            allEntries.RemoveAll(entry => entry.isPlayer && entry != bestPlayer);
        }
    }

    // Oyun durumunu güncelle
    public static void UpdateGameState(bool gameActive, int activePlayers, int totalPlayers)
    {
        isGameActive = gameActive;
        activePlayerCount = activePlayers;
        totalPlayerCount = totalPlayers;
    }

    // Oyuncu öldüğünde çağrılacak metod
    public static void PlayerDied(string playerName)
    {
        var entry = allEntries.Find(e => e.name == playerName && e.isPlayer);
        if (entry != null)
        {
            entry.isDead = true;
        }
        
        // Instance'ın isDead özelliğini de güncelle
        if (Instance != null)
        {
            Instance.isDead = true;
        }
    }

    // Son 5 oyuncu kaldığında sıralamayı korumak için kontrol
    public static bool ShouldPreserveLeaderboard()
    {
        return activePlayerCount <= 5 && isGameActive;
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;
    public bool isPlayer;
    public bool isDead = false; // Yeni: ölen oyuncuları takip etmek için
}
