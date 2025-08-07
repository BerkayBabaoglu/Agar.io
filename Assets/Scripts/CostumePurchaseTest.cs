using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test script for costume purchase functionality
/// </summary>
public class CostumePurchaseTest : MonoBehaviour
{
    [Header("Test Controls")]
    public Button addMoneyButton;
    public Button testKaanButton;
    public Button testKeremButton;
    public Button testKuzeyButton;
    public Button resetButton;
    
    [Header("Test Amounts")]
    public int addAmount = 300;
    
    private void Start()
    {
        SetupTestButtons();
    }
    
    private void SetupTestButtons()
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
                if (PlayerSkinSelect.Instance != null)
                {
                    PlayerSkinSelect.Instance.SelectKuzey();
                }
            });
        }
        
        // Reset Button
        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(() => {
                if (CostumeShop.Instance != null)
                {
                    CostumeShop.Instance.ResetCostumeData();
                }
                if (CurrencyManager.Instance != null)
                {
                    CurrencyManager.Instance.ResetCurrencyData();
                }
                Debug.Log("All data reset!");
            });
        }
    }
    
    [ContextMenu("Test Kaan Purchase")]
    public void TestKaanPurchase()
    {
        if (PlayerSkinSelect.Instance != null)
        {
            PlayerSkinSelect.Instance.SelectKaan();
        }
    }
    
    [ContextMenu("Test Kerem Purchase")]
    public void TestKeremPurchase()
    {
        if (PlayerSkinSelect.Instance != null)
        {
            PlayerSkinSelect.Instance.SelectKerem();
        }
    }
    
    [ContextMenu("Test Kuzey Purchase")]
    public void TestKuzeyPurchase()
    {
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
    
    [ContextMenu("Show Current Status")]
    public void ShowCurrentStatus()
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