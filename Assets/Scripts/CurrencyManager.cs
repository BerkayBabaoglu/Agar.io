using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class CurrencyData
{
    public int money = 0;
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    [Header("UI References")]
    public TextMeshProUGUI parabarText; // parabar text component
    public TextMeshProUGUI parabar1Text; // parabar (1) text component
    
    [Header("Currency Settings")]
    public int scoreToMoneyConversionRate = 1; // 1 score = 1 money
    
    private CurrencyData currencyData;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Set save file path to persistent data path
            saveFilePath = Path.Combine(Application.persistentDataPath, "currency_data.json");
            
            // Load saved currency data
            LoadCurrencyData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Auto-find parabar texts if not set
        if (parabarText == null || parabar1Text == null)
        {
            FindParabarText();
        }
        
        // If parabar1Text is still null, try one more time after a short delay
        if (parabar1Text == null)
        {
            StartCoroutine(DelayedFindParabar1Text());
        }
        
        // Load currency data and update UI to ensure JSON data is displayed
        LoadAndUpdateUI();
    }
    
    private System.Collections.IEnumerator DelayedFindParabar1Text()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (parabar1Text == null)
        {
            Debug.Log("Trying delayed search for parabar (1) text...");
            FindParabar1Text();
        }
    }
    
    /// <summary>
    /// Automatically find the parabar text components in the scene
    /// </summary>
    private void FindParabarText()
    {
        // Find the parabar GameObject
        GameObject parabar = GameObject.Find("parabar");
        if (parabar != null)
        {
            // Find the Text (TMP) child component
            parabarText = parabar.GetComponentInChildren<TextMeshProUGUI>();
            if (parabarText != null)
            {
                Debug.Log("Successfully found parabar text component!");
            }
            else
            {
                Debug.LogWarning("Found parabar GameObject but couldn't find TextMeshProUGUI component in children!");
            }
        }
        else
        {
            Debug.LogWarning("Could not find parabar GameObject in scene!");
        }
        
        // Find the parabar (1) GameObject - try multiple search methods
        GameObject parabar1 = null;
        
        // Method 1: Direct search
        parabar1 = GameObject.Find("parabar (1)");
        
        // Method 2: Search with different naming variations
        if (parabar1 == null)
        {
            parabar1 = GameObject.Find("parabar(1)");
        }
        
        // Method 3: Search by partial name
        if (parabar1 == null)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("parabar") && obj.name.Contains("1"))
                {
                    parabar1 = obj;
                    Debug.Log($"Found parabar (1) with name: {obj.name}");
                    break;
                }
            }
        }
        
        // Method 4: Search in inactive objects too
        if (parabar1 == null)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("parabar") && obj.name.Contains("1") && obj.scene.name == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    parabar1 = obj;
                    Debug.Log($"Found parabar (1) in inactive objects: {obj.name}");
                    break;
                }
            }
        }
        
        // Method 5: Search by TextMeshProUGUI components that might be in parabar (1)
        if (parabar1 == null)
        {
            TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in allTexts)
            {
                GameObject parent = text.gameObject;
                while (parent != null)
                {
                    if (parent.name.Contains("parabar") && parent.name.Contains("1"))
                    {
                        parabar1 = parent;
                        Debug.Log($"Found parabar (1) by searching through TextMeshProUGUI: {parent.name}");
                        break;
                    }
                    parent = parent.transform.parent?.gameObject;
                }
                if (parabar1 != null) break;
            }
        }
        
        if (parabar1 != null)
        {
            // Find the Text (TMP) child component
            parabar1Text = parabar1.GetComponentInChildren<TextMeshProUGUI>();
            if (parabar1Text != null)
            {
                Debug.Log($"Successfully found parabar (1) text component in {parabar1.name}!");
            }
            else
            {
                Debug.LogWarning($"Found parabar (1) GameObject ({parabar1.name}) but couldn't find TextMeshProUGUI component in children!");
            }
        }
        else
        {
            Debug.LogWarning("Could not find parabar (1) GameObject in scene using any search method!");
            Debug.LogWarning("Available GameObjects with 'parabar' in name:");
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("parabar"))
                {
                    Debug.LogWarning($"Found: {obj.name}");
                }
            }
        }
    }

    /// <summary>
    /// Converts player's score to money when they die
    /// </summary>
    /// <param name="finalScore">The score the player achieved before dying</param>
    public void ConvertScoreToMoney(int finalScore)
    {
        int earnedMoney = finalScore * scoreToMoneyConversionRate;
        currencyData.money += earnedMoney;
        
        Debug.Log($"Player died with score: {finalScore}, earned money: {earnedMoney}, total money: {currencyData.money}");
        
        // Save the updated currency data
        SaveCurrencyData();
        
        // Update UI
        UpdateParabarUI();
    }

    /// <summary>
    /// Spend money (for future shop functionality)
    /// </summary>
    /// <param name="amount">Amount of money to spend</param>
    /// <returns>True if successful, false if not enough money</returns>
    public bool SpendMoney(int amount)
    {
        if (currencyData.money >= amount)
        {
            currencyData.money -= amount;
            SaveCurrencyData();
            UpdateParabarUI();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Add money directly (for testing or other purposes)
    /// </summary>
    /// <param name="amount">Amount of money to add</param>
    public void AddMoney(int amount)
    {
        currencyData.money += amount;
        SaveCurrencyData();
        UpdateParabarUI();
    }

    /// <summary>
    /// Get current money amount
    /// </summary>
    /// <returns>Current money amount</returns>
    public int GetMoney()
    {
        return currencyData.money;
    }

    /// <summary>
    /// Update the parabar UI texts to show current money
    /// </summary>
    private void UpdateParabarUI()
    {
        string moneyText = currencyData.money.ToString();
        
        if (parabarText != null)
        {
            parabarText.text = moneyText;
        }
        else
        {
            Debug.LogWarning("Parabar text reference is not set in CurrencyManager!");
        }
        
        if (parabar1Text != null)
        {
            parabar1Text.text = moneyText;
        }
        else
        {
            Debug.LogWarning("Parabar (1) text reference is not set in CurrencyManager!");
        }
    }
    
    /// <summary>
    /// Refresh the UI (public method that can be called from other scripts)
    /// </summary>
    public void RefreshUI()
    {
        // Re-find parabar texts if needed
        if (parabarText == null || parabar1Text == null)
        {
            FindParabarText();
        }
        
        // Load fresh data from JSON and update UI
        LoadAndUpdateUI();
    }

    /// <summary>
    /// Save currency data to JSON file
    /// </summary>
    private void SaveCurrencyData()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(currencyData, true);
            File.WriteAllText(saveFilePath, jsonData);
            Debug.Log($"Currency data saved: {currencyData.money} money");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save currency data: {e.Message}");
        }
    }

    /// <summary>
    /// Load currency data from JSON file
    /// </summary>
    private void LoadCurrencyData()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                currencyData = JsonUtility.FromJson<CurrencyData>(jsonData);
                Debug.Log($"Currency data loaded: {currencyData.money} money");
            }
            else
            {
                // Create new currency data if file doesn't exist
                currencyData = new CurrencyData();
                Debug.Log("No existing currency data found, created new data");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load currency data: {e.Message}");
            // Create new currency data as fallback
            currencyData = new CurrencyData();
        }
    }
    
    /// <summary>
    /// Load currency data and update UI (called after UI is ready)
    /// </summary>
    public void LoadAndUpdateUI()
    {
        LoadCurrencyData();
        UpdateParabarUI();
    }

    /// <summary>
    /// Force save currency data (public method)
    /// </summary>
    public void ForceSave()
    {
        SaveCurrencyData();
    }
    
    /// <summary>
    /// Reset all currency data (for testing)
    /// </summary>
    [ContextMenu("Reset Currency Data")]
    public void ResetCurrencyData()
    {
        currencyData = new CurrencyData();
        SaveCurrencyData();
        UpdateParabarUI();
        Debug.Log("Currency data reset");
    }
    
    /// <summary>
    /// Manually find and set parabar (1) text reference
    /// </summary>
    [ContextMenu("Find Parabar (1) Text")]
    public void FindParabar1Text()
    {
        Debug.Log("Searching for parabar (1) text component...");
        
        // Try to find the text component
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in allTexts)
        {
            GameObject parent = text.gameObject;
            while (parent != null)
            {
                if (parent.name.Contains("parabar") && parent.name.Contains("1"))
                {
                    parabar1Text = text;
                    Debug.Log($"Successfully set parabar1Text to: {text.name} in {parent.name}");
                    UpdateParabarUI();
                    return;
                }
                parent = parent.transform.parent?.gameObject;
            }
        }
        
        Debug.LogWarning("Could not find parabar (1) text component!");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveCurrencyData();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveCurrencyData();
        }
    }

    private void OnDestroy()
    {
        SaveCurrencyData();
    }
}