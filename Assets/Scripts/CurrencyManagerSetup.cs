using UnityEngine;

/// <summary>
/// Helper script to easily set up the CurrencyManager in the scene
/// Attach this to any GameObject to automatically create and configure CurrencyManager
/// </summary>
public class CurrencyManagerSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    [Tooltip("If checked, this will automatically create CurrencyManager instance on start")]
    public bool autoSetup = true;
    
    private void Start()
    {
        if (autoSetup)
        {
            SetupCurrencyManager();
        }
    }
    
    [ContextMenu("Setup Currency Manager")]
    public void SetupCurrencyManager()
    {
        // Check if CurrencyManager already exists
        if (CurrencyManager.Instance != null)
        {
            Debug.Log("CurrencyManager already exists in the scene.");
            return;
        }
        
        // Create new GameObject for CurrencyManager
        GameObject currencyManagerObj = new GameObject("CurrencyManager");
        
        // Add CurrencyManager component
        CurrencyManager currencyManager = currencyManagerObj.AddComponent<CurrencyManager>();
        
        Debug.Log("CurrencyManager has been created and set up successfully!");
        Debug.Log("Currency system is now active. Player score will be converted to money when they die.");
        Debug.Log("Money will be displayed in the parabar UI and saved to JSON automatically.");
        
        // Don't destroy this object since we've set it up
        // The CurrencyManager itself handles DontDestroyOnLoad
        Destroy(this);
    }
}