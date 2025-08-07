using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple debug script to test costume system
/// </summary>
public class CostumeDebug : MonoBehaviour
{
    [Header("Debug Buttons")]
    public Button testKaanButton;
    public Button testKeremButton;
    public Button testKuzeyButton;
    public Button addMoneyButton;
    
    [Header("Debug Amounts")]
    public int addAmount = 300;
    
    private void Start()
    {
        SetupDebugButtons();
    }
    
    private void SetupDebugButtons()
    {
        // Add Money Button
        if (addMoneyButton != null)
        {
            addMoneyButton.onClick.RemoveAllListeners();
            addMoneyButton.onClick.AddListener(() => {
                if (CurrencyManager.Instance != null)
                {
                    CurrencyManager.Instance.AddMoney(addAmount);
                    Debug.Log($"Added {addAmount} money. Total: {CurrencyManager.Instance.GetMoney()}");
                }
            });
        }
        
        // Test Kaan Button
        if (testKaanButton != null)
        {
            testKaanButton.onClick.RemoveAllListeners();
            testKaanButton.onClick.AddListener(() => {
                Debug.Log("Testing Kaan costume...");
                if (PlayerSkinSelect.Instance != null)
                {
                    PlayerSkinSelect.Instance.SelectKaan();
                }
            });
        }
        
        // Test Kerem Button
        if (testKeremButton != null)
        {
            testKeremButton.onClick.RemoveAllListeners();
            testKeremButton.onClick.AddListener(() => {
                Debug.Log("Testing Kerem costume...");
                if (PlayerSkinSelect.Instance != null)
                {
                    PlayerSkinSelect.Instance.SelectKerem();
                }
            });
        }
        
        // Test Kuzey Button
        if (testKuzeyButton != null)
        {
            testKuzeyButton.onClick.RemoveAllListeners();
            testKuzeyButton.onClick.AddListener(() => {
                Debug.Log("Testing Kuzey costume...");
                if (PlayerSkinSelect.Instance != null)
                {
                    PlayerSkinSelect.Instance.SelectKuzey();
                }
            });
        }
    }
    
    [ContextMenu("Test Kaan")]
    public void TestKaan()
    {
        Debug.Log("Testing Kaan costume via ContextMenu...");
        if (PlayerSkinSelect.Instance != null)
        {
            PlayerSkinSelect.Instance.SelectKaan();
        }
    }
    
    [ContextMenu("Test Kerem")]
    public void TestKerem()
    {
        Debug.Log("Testing Kerem costume via ContextMenu...");
        if (PlayerSkinSelect.Instance != null)
        {
            PlayerSkinSelect.Instance.SelectKerem();
        }
    }
    
    [ContextMenu("Test Kuzey")]
    public void TestKuzey()
    {
        Debug.Log("Testing Kuzey costume via ContextMenu...");
        if (PlayerSkinSelect.Instance != null)
        {
            PlayerSkinSelect.Instance.SelectKuzey();
        }
    }
    
    [ContextMenu("Add Money")]
    public void AddMoney()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddMoney(addAmount);
            Debug.Log($"Added {addAmount} money. Total: {CurrencyManager.Instance.GetMoney()}");
        }
    }
    
    [ContextMenu("Show Status")]
    public void ShowStatus()
    {
        if (CurrencyManager.Instance != null)
        {
            Debug.Log($"Current money: {CurrencyManager.Instance.GetMoney()}");
        }
        
        if (CostumeShop.Instance != null)
        {
            Debug.Log($"Kaan unlocked: {CostumeShop.Instance.IsCostumeUnlocked("kaan")}");
            Debug.Log($"Kerem unlocked: {CostumeShop.Instance.IsCostumeUnlocked("kerem")}");
            Debug.Log($"Kuzey unlocked: {CostumeShop.Instance.IsCostumeUnlocked("kuzey")}");
        }
    }
} 