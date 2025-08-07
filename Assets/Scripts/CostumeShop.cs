using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class CostumeData
{
    public bool kaanUnlocked = false;
    public bool keremUnlocked = false;
    public bool kuzeyUnlocked = false;
    public string selectedCostume = "default";
}

[System.Serializable]
public class CostumeInfo
{
    public string costumeName;
    public int price;
    public Sprite sprite;
    public string methodName; // PlayerSkinSelect method name
}

public class CostumeShop : MonoBehaviour
{
    public static CostumeShop Instance { get; private set; }
    
    [Header("Costume Buttons")]
    public Button kaanButton;
    public Button keremButton;
    public Button kuzeyButton;
    
    [Header("Price Texts (Optional - Auto-find if null)")]
    public TextMeshProUGUI kaanPriceText;
    public TextMeshProUGUI keremPriceText;
    public TextMeshProUGUI kuzeyPriceText;
    
    [Header("Costume Settings")]
    public int kaanPrice = 250;
    public int keremPrice = 250;
    public int kuzeyPrice = 250;
    
    [Header("Visual Feedback")]
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;
    public Color selectedColor = Color.green;
    
    private CostumeData costumeData;
    private string saveFilePath;
    private PlayerSkinSelect playerSkinSelect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Set save file path
            saveFilePath = Path.Combine(Application.persistentDataPath, "costume_data.json");
            
            // Load costume data
            LoadCostumeData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find PlayerSkinSelect component
        playerSkinSelect = FindObjectOfType<PlayerSkinSelect>();
        
        // Auto-find UI components if not set
        AutoFindUIComponents();
        
        // Set up button events
        SetupButtonEvents();
        
        // Update UI
        UpdateCostumeUI();
    }
    
    private void AutoFindUIComponents()
    {
        // Auto-find buttons if not set
        if (kaanButton == null)
        {
            GameObject kaanObj = GameObject.Find("kaan");
            if (kaanObj != null) kaanButton = kaanObj.GetComponent<Button>();
        }
        
        if (keremButton == null)
        {
            GameObject keremObj = GameObject.Find("kerem");
            if (keremObj != null) keremButton = keremObj.GetComponent<Button>();
        }
        
        if (kuzeyButton == null)
        {
            GameObject kuzeyObj = GameObject.Find("kuzey");
            if (kuzeyObj != null) kuzeyButton = kuzeyObj.GetComponent<Button>();
        }
        
        // Auto-find price texts if not set (look for "250" text components)
        if (kaanPriceText == null && kaanButton != null)
        {
            kaanPriceText = kaanButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (keremPriceText == null && keremButton != null)
        {
            keremPriceText = keremButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (kuzeyPriceText == null && kuzeyButton != null)
        {
            kuzeyPriceText = kuzeyButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        Debug.Log($"CostumeShop: Found {(kaanButton != null ? "Kaan" : "")} {(keremButton != null ? "Kerem" : "")} {(kuzeyButton != null ? "Kuzey" : "")} buttons");
    }

    private void SetupButtonEvents()
    {
        if (kaanButton != null)
        {
            kaanButton.onClick.RemoveAllListeners();
            kaanButton.onClick.AddListener(() => HandleCostumeClick("kaan"));
        }
        
        if (keremButton != null)
        {
            keremButton.onClick.RemoveAllListeners();
            keremButton.onClick.AddListener(() => HandleCostumeClick("kerem"));
        }
        
        if (kuzeyButton != null)
        {
            kuzeyButton.onClick.RemoveAllListeners();
            kuzeyButton.onClick.AddListener(() => HandleCostumeClick("kuzey"));
        }
    }

    private void HandleCostumeClick(string costumeName)
    {
        bool isUnlocked = IsCostumeUnlocked(costumeName);
        
        if (isUnlocked)
        {
            // Select the costume
            SelectCostume(costumeName);
        }
        else
        {
            // Try to buy the costume
            TryBuyCostume(costumeName);
        }
    }

    public bool IsCostumeUnlocked(string costumeName)
    {
        switch (costumeName.ToLower())
        {
            case "kaan":
                return costumeData.kaanUnlocked;
            case "kerem":
                return costumeData.keremUnlocked;
            case "kuzey":
                return costumeData.kuzeyUnlocked;
            default:
                return false;
        }
    }

    private int GetCostumePrice(string costumeName)
    {
        switch (costumeName.ToLower())
        {
            case "kaan":
                return kaanPrice;
            case "kerem":
                return keremPrice;
            case "kuzey":
                return kuzeyPrice;
            default:
                return 0;
        }
    }

    private void TryBuyCostume(string costumeName)
    {
        int price = GetCostumePrice(costumeName);
        
        if (CurrencyManager.Instance != null)
        {
            if (CurrencyManager.Instance.SpendMoney(price))
            {
                // Successfully bought
                UnlockCostume(costumeName);
                SelectCostume(costumeName);
                
                Debug.Log($"Successfully bought {costumeName} costume for {price} coins!");
                
                // Show success feedback
                ShowPurchaseSuccess(costumeName);
            }
            else
            {
                // Not enough money
                Debug.Log($"Not enough money to buy {costumeName} costume. Need {price} coins.");
                
                // Show failure feedback
                ShowPurchaseFailure(costumeName);
            }
        }
        else
        {
            Debug.LogError("CurrencyManager not found!");
        }
    }

    public void UnlockCostume(string costumeName)
    {
        switch (costumeName.ToLower())
        {
            case "kaan":
                costumeData.kaanUnlocked = true;
                break;
            case "kerem":
                costumeData.keremUnlocked = true;
                break;
            case "kuzey":
                costumeData.kuzeyUnlocked = true;
                break;
        }
        
        SaveCostumeData();
        UpdateCostumeUI();
    }

    public void SelectCostume(string costumeName)
    {
        costumeData.selectedCostume = costumeName;
        SaveCostumeData();
        
        // Apply the costume to the player
        ApplyCostume(costumeName);
        
        // Update UI to show selection
        UpdateCostumeUI();
        
        Debug.Log($"Selected {costumeName} costume!");
    }

    private void ApplyCostume(string costumeName)
    {
        if (playerSkinSelect == null)
        {
            playerSkinSelect = FindObjectOfType<PlayerSkinSelect>();
        }
        
        if (playerSkinSelect != null)
        {
            // Directly apply the costume without calling the purchase methods
            switch (costumeName.ToLower())
            {
                case "kaan":
                    playerSkinSelect.ApplyCostumeDirectly(0);
                    break;
                case "kerem":
                    playerSkinSelect.ApplyCostumeDirectly(1);
                    break;
                case "kuzey":
                    playerSkinSelect.ApplyCostumeDirectly(2);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("PlayerSkinSelect component not found!");
        }
    }

    private void UpdateCostumeUI()
    {
        // Update Kaan UI
        UpdateCostumeButtonUI("kaan", kaanButton, kaanPriceText, null);
        
        // Update Kerem UI
        UpdateCostumeButtonUI("kerem", keremButton, keremPriceText, null);
        
        // Update Kuzey UI
        UpdateCostumeButtonUI("kuzey", kuzeyButton, kuzeyPriceText, null);
    }

    private void UpdateCostumeButtonUI(string costumeName, Button button, TextMeshProUGUI priceText, TextMeshProUGUI buyText)
    {
        if (button == null) return;
        
        bool isUnlocked = IsCostumeUnlocked(costumeName);
        bool isSelected = costumeData.selectedCostume == costumeName;
        int price = GetCostumePrice(costumeName);
        
        // Update button interactability
        button.interactable = true;
        
        // Update button visual state
        UpdateButtonVisuals(button, isSelected, isUnlocked);
        
        // Update price text - for now we'll keep showing the price
        if (priceText != null)
        {
            if (isUnlocked)
            {
                if (isSelected)
                {
                    priceText.text = "✓"; // Check mark for selected
                    priceText.color = selectedColor;
                }
                else
                {
                    priceText.text = "OWNED";
                    priceText.color = unlockedColor;
                }
            }
            else
            {
                priceText.text = price.ToString();
                priceText.color = Color.white;
            }
        }
    }
    
    private void UpdateButtonVisuals(Button button, bool isSelected, bool isUnlocked)
    {
        // Try to find and update button background
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (isSelected)
            {
                buttonImage.color = selectedColor;
            }
            else if (isUnlocked)
            {
                buttonImage.color = unlockedColor;
            }
            else
            {
                buttonImage.color = lockedColor;
            }
        }
        
        // Also try to update the costume image itself
        Image[] childImages = button.GetComponentsInChildren<Image>();
        foreach (Image img in childImages)
        {
            if (img != buttonImage) // Don't modify the button background again
            {
                if (isUnlocked)
                {
                    img.color = Color.white; // Full color for unlocked
                }
                else
                {
                    img.color = lockedColor; // Grayed out for locked
                }
            }
        }
    }

    public void ShowPurchaseSuccess(string costumeName)
    {
        // Add visual/audio feedback for successful purchase
        Debug.Log($"🎉 Successfully purchased {costumeName}!");
        
        // Find the button and play success animation
        Button costumeButton = GetCostumeButton(costumeName);
        if (costumeButton != null)
        {
            // Use simple animation manager
            SimpleAnimationManager.Instance.SimpleScale(costumeButton.transform, 1.1f, 0.2f);
        }
    }

    public void ShowPurchaseFailure(string costumeName)
    {
        // Add visual/audio feedback for failed purchase
        Debug.Log($"❌ Not enough money to buy {costumeName}!");
        
        // Find the button and play shake animation
        Button costumeButton = GetCostumeButton(costumeName);
        if (costumeButton != null)
        {
            // Use simple animation manager
            SimpleAnimationManager.Instance.SimpleShake(costumeButton.transform, 0.5f, 0.1f);
        }
    }
    
    private System.Collections.IEnumerator FlashButtonColor(Button button, Color flashColor)
    {
        if (button == null) yield break;
        
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage == null) yield break;
        
        Color originalColor = buttonImage.color;
        buttonImage.color = flashColor;
        
        yield return new WaitForSeconds(0.2f);
        
        buttonImage.color = originalColor;
    }
    
    private Button GetCostumeButton(string costumeName)
    {
        switch (costumeName.ToLower())
        {
            case "kaan":
                return kaanButton;
            case "kerem":
                return keremButton;
            case "kuzey":
                return kuzeyButton;
            default:
                return null;
        }
    }

    /// <summary>
    /// Load costume data from JSON file
    /// </summary>
    private void LoadCostumeData()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                costumeData = JsonUtility.FromJson<CostumeData>(jsonData);
                Debug.Log("Costume data loaded successfully!");
            }
            else
            {
                // Create new costume data if file doesn't exist
                costumeData = new CostumeData();
                Debug.Log("No existing costume data found, created new data");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load costume data: {e.Message}");
            // Create new costume data as fallback
            costumeData = new CostumeData();
        }
    }

    /// <summary>
    /// Save costume data to JSON file
    /// </summary>
    private void SaveCostumeData()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(costumeData, true);
            File.WriteAllText(saveFilePath, jsonData);
            Debug.Log("Costume data saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save costume data: {e.Message}");
        }
    }

    /// <summary>
    /// Apply the last selected costume on game start
    /// </summary>
    public void ApplySelectedCostume()
    {
        if (!string.IsNullOrEmpty(costumeData.selectedCostume) && 
            costumeData.selectedCostume != "default" &&
            IsCostumeUnlocked(costumeData.selectedCostume))
        {
            ApplyCostume(costumeData.selectedCostume);
        }
    }

    /// <summary>
    /// Reset all costume data (for testing)
    /// </summary>
    [ContextMenu("Reset Costume Data")]
    public void ResetCostumeData()
    {
        costumeData = new CostumeData();
        SaveCostumeData();
        UpdateCostumeUI();
        Debug.Log("Costume data reset!");
    }

    /// <summary>
    /// Unlock all costumes (for testing)
    /// </summary>
    [ContextMenu("Unlock All Costumes")]
    public void UnlockAllCostumes()
    {
        costumeData.kaanUnlocked = true;
        costumeData.keremUnlocked = true;
        costumeData.kuzeyUnlocked = true;
        SaveCostumeData();
        UpdateCostumeUI();
        Debug.Log("All costumes unlocked!");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveCostumeData();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveCostumeData();
        }
    }

    private void OnDestroy()
    {
        SaveCostumeData();
        
        // Clean up all DOTween animations when object is destroyed
        CleanupAllDOTweens();
    }
    
    private void OnDisable()
    {
        // Clean up all DOTween animations when object is disabled
        CleanupAllDOTweens();
    }
    
    private void CleanupAllDOTweens()
    {
        // No cleanup needed for simple animations
    }
}