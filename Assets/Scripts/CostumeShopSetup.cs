using UnityEngine;

/// <summary>
/// Helper script to easily set up the CostumeShop in the scene
/// Attach this to any GameObject to automatically create and configure CostumeShop
/// </summary>
public class CostumeShopSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    [Tooltip("If checked, this will automatically create CostumeShop instance on start")]
    public bool autoSetup = true;
    
    private void Start()
    {
        if (autoSetup)
        {
            SetupCostumeShop();
        }
    }
    
    [ContextMenu("Setup Costume Shop")]
    public void SetupCostumeShop()
    {
        // Check if CostumeShop already exists
        if (CostumeShop.Instance != null)
        {
            Debug.Log("CostumeShop already exists in the scene.");
            return;
        }
        
        // Create new GameObject for CostumeShop
        GameObject costumeShopObj = new GameObject("CostumeShop");
        
        // Add CostumeShop component
        CostumeShop costumeShop = costumeShopObj.AddComponent<CostumeShop>();
        
        Debug.Log("CostumeShop has been created and set up successfully!");
        Debug.Log("Costume purchase system is now active!");
        Debug.Log("Players can now buy costumes using money earned from gameplay.");
        
        // Don't destroy this object since we've set it up
        // The CostumeShop itself handles DontDestroyOnLoad
        Destroy(this);
    }
}