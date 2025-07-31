using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Game;
    public GameObject Menu;

    public CharacterMovement character;

    private void Awake()
    {
        Instance = this;
    }

    public void GameLoad()
    {
        Game.SetActive(true);
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.isReloading = false;
        
        // Oyun başladığında lider tablosunu kontrol et
        CheckAndClearLeaderboard();
        
        // Oyun durumunu aktif olarak işaretle ve aktif oyuncu sayısını hesapla
        UpdateActivePlayerCount();
    }
    
    void UpdateActivePlayerCount()
    {
        int activePlayers = 0;
        int totalPlayers = 0;
        
        // Düşmanları say
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
    
    public void MenuLoad()
    {
        Menu.SetActive(true);
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.isReloading = false;

        if (Menu.gameObject.activeSelf == true)
        {
            character.gameObject.SetActive(true);
            
            ScoreManager.Instance.score = 0;
            ScoreManager.Instance.UpdateScoreText();
            
            character.currentScale = 0.15f;
            character.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
            
            if (CameraFollow.Instance != null)
            {
                CameraFollow.Instance.m_Camera.orthographicSize = 2f;
                CameraFollow.Instance.targetSize = 2f;
            }
            
            character.StartInvulnerability();

            Debug.Log("Menu Load");
        }
    }

    public void GameDeactive()
    {
        Color color = CameraFollow.Instance.panel.color;
        color.a = 0f;
        CameraFollow.Instance.panel.color = color;

        Game.SetActive(false);

        Debug.Log("GameDeactive");
    }

    public void MenuDeactive() { 
        Game.SetActive(false);
    }

    void CheckAndClearLeaderboard()
    {
        // Eğer son 5 oyuncu kaldıysa lider tablosunu koru
        if (ScoreManager.ShouldPreserveLeaderboard())
        {
            Debug.Log("Son 5 oyuncu kaldı, lider tablosu korunuyor");
            return;
        }
        
        // Aksi takdirde lider tablosunu temizle
        ScoreManager.allEntries.Clear();
        
        // Düşman listesini de temizle
        EnemyManager.allEnemies.Clear();
        
        Debug.Log("Lider tablosu temizlendi");
    }
}
