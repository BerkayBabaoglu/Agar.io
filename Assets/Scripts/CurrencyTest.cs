using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test script for currency system - attach to any GameObject to test
/// </summary>
public class CurrencyTest : MonoBehaviour
{
    [Header("Test Controls")]
    public Button addMoneyButton;
    public Button spendMoneyButton;
    public Button resetButton;
    public Button refreshButton;
    
    [Header("Test Amounts")]
    public int addAmount = 100;
    public int spendAmount = 50;
    
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
        
        // Spend Money Button
        if (spendMoneyButton != null)
        {
            spendMoneyButton.onClick.RemoveAllListeners();
            spendMoneyButton.onClick.AddListener(() => {
                if (CurrencyManager.Instance != null)
                {
                    bool success = CurrencyManager.Instance.SpendMoney(spendAmount);
                    if (success)
                    {
                        Debug.Log($"Spent {spendAmount} money. Total: {CurrencyManager.Instance.GetMoney()}");
                    }
                    else
                    {
                        Debug.Log($"Not enough money to spend {spendAmount}. Current: {CurrencyManager.Instance.GetMoney()}");
                    }
                }
            });
        }
        
        // Reset Button
        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(() => {
                if (CurrencyManager.Instance != null)
                {
                    CurrencyManager.Instance.ResetCurrencyData();
                    Debug.Log("Currency data reset!");
                }
            });
        }
        
        // Refresh Button
        if (refreshButton != null)
        {
            refreshButton.onClick.RemoveAllListeners();
            refreshButton.onClick.AddListener(() => {
                if (CurrencyManager.Instance != null)
                {
                    CurrencyManager.Instance.RefreshUI();
                    Debug.Log($"Refreshed UI. Current money: {CurrencyManager.Instance.GetMoney()}");
                }
            });
        }
    }
    
    [ContextMenu("Test Add Money")]
    public void TestAddMoney()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddMoney(addAmount);
            Debug.Log($"Test: Added {addAmount} money. Total: {CurrencyManager.Instance.GetMoney()}");
        }
    }
    
    [ContextMenu("Test Spend Money")]
    public void TestSpendMoney()
    {
        if (CurrencyManager.Instance != null)
        {
            bool success = CurrencyManager.Instance.SpendMoney(spendAmount);
            Debug.Log($"Test: Spend {spendAmount} money - Success: {success}, Total: {CurrencyManager.Instance.GetMoney()}");
        }
    }
    
    [ContextMenu("Test Refresh UI")]
    public void TestRefreshUI()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.RefreshUI();
            Debug.Log($"Test: Refreshed UI. Current money: {CurrencyManager.Instance.GetMoney()}");
        }
    }
    
    [ContextMenu("Show Current Money")]
    public void ShowCurrentMoney()
    {
        if (CurrencyManager.Instance != null)
        {
            Debug.Log($"Current money: {CurrencyManager.Instance.GetMoney()}");
        }
        else
        {
            Debug.LogWarning("CurrencyManager not found!");
        }
    }
} 