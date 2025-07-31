using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Lider Tablosu UI Elemanları")]
    public TextMeshProUGUI[] rankEntries; // İlk 5 sıra için (en yüksek puanlı oyuncular/düşmanlar)
    public TextMeshProUGUI playerEntry; // Oyuncu bilgilendirme sırası (en altta)
    public GameObject playerEntryContainer; // Oyuncu bilgilendirme container'ı (aktif/pasif için)

    [Header("Ayarlar")]
    public int maxTopEntries = 5; // İlk kaç sırayı göstereceğiz

    private void Start()
    {
        // Başlangıçta oyuncu bilgilendirme alanını aktif et
        if (playerEntryContainer != null)
        {
            playerEntryContainer.SetActive(true);
        }
    }

    private void Update()
    {
        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        if (ScoreManager.Instance == null) return;

        // Tekrar oyuncu girişlerini temizle
        ScoreManager.ClearDuplicatePlayers();

        // Tüm girişleri puana göre sırala (oyuncular + düşmanlar)
        var sortedEntries = ScoreManager.GetSortedLeaderboard();
        
        // İlk 5 sırayı göster (en yüksek puanlılar)
        for (int i = 0; i < rankEntries.Length && i < maxTopEntries; i++)
        {
            if (i < sortedEntries.Count)
            {
                var entry = sortedEntries[i];
                string rankText = $"{i + 1}. {entry.name} - {entry.score}";
                
                // Eğer bu oyuncu ise "(Siz)" etiketi ekle
                if (entry.isPlayer)
                {
                    rankText += " (Siz)";
                    
                    // Eğer oyuncu öldüyse "Öldü" yazısı ekle
                    if (entry.isDead)
                    {
                        rankText += " - Öldü";
                    }
                }
                
                rankEntries[i].text = rankText;
                rankEntries[i].gameObject.SetActive(true);
            }
            else
            {
                rankEntries[i].gameObject.SetActive(false);
            }
        }

        // Oyuncu bilgilendirme sistemi
        UpdatePlayerInfo(sortedEntries);
    }

    void UpdatePlayerInfo(List<LeaderboardEntry> sortedEntries)
    {
        int playerRank = ScoreManager.GetPlayerRank();
        
        if (playerRank > 0)
        {
            if (playerRank <= maxTopEntries)
            {
                // Oyuncu ilk 5'te → Bilgilendirme alanını gizle
                if (playerEntryContainer != null)
                {
                    playerEntryContainer.SetActive(false);
                }
                if (playerEntry != null)
                {
                    playerEntry.gameObject.SetActive(false);
                }
            }
            else
            {
                // Oyuncu ilk 5'te değil → Bilgilendirme alanını göster
                if (playerEntryContainer != null)
                {
                    playerEntryContainer.SetActive(true);
                }
                if (playerEntry != null)
                {
                    playerEntry.gameObject.SetActive(true);
                    var playerEntryData = sortedEntries.Find(entry => entry.name == ScoreManager.Instance.playerName && entry.isPlayer);
                    if (playerEntryData != null)
                    {
                        // Oyuncunun mevcut sırasını ve puanını göster
                        string playerText = $"{playerRank}. {playerEntryData.name} - {playerEntryData.score} (Siz)";
                        
                        // Eğer oyuncu öldüyse "Öldü" yazısı ekle
                        if (playerEntryData.isDead)
                        {
                            playerText += " - Öldü";
                        }
                        
                        playerEntry.text = playerText;
                    }
                    else
                    {
                        playerEntry.text = "Oyuncu bilgisi bulunamadı";
                    }
                }
            }
        }
        else
        {
            // Oyuncu henüz lider tablosunda yok
            if (playerEntryContainer != null)
            {
                playerEntryContainer.SetActive(true);
            }
            if (playerEntry != null)
            {
                playerEntry.gameObject.SetActive(true);
                playerEntry.text = "Henüz puanınız yok";
            }
        }
    }
}
