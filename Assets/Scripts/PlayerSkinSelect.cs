using UnityEngine;
using System.Collections;

public class PlayerSkinSelect : MonoBehaviour
{
    public static PlayerSkinSelect Instance { get; private set; }
    
    public GameObject player;
    private SpriteRenderer playerSprite;
    public Sprite[] customSprite;
    
    [Header("Purchase Settings")]
    public int kaanPrice = 250;
    public int keremPrice = 250;
    public int kuzeyPrice = 250;
    
    [Header("Animation Settings")]
    public float shakeDuration = 0.5f;
    public float shakeStrength = 10f;
    public float successScaleDuration = 0.3f;
    public float successScaleMultiplier = 1.2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        
        // Apply the last selected costume on start
        if (CostumeShop.Instance != null)
        {
            CostumeShop.Instance.ApplySelectedCostume();
        }
    }

    public void SelectKaan()
    {
        TryPurchaseCostume("kaan", kaanPrice, 0);
    }

    public void SelectKerem()
    {
        TryPurchaseCostume("kerem", keremPrice, 1);
    }

    public void SelectKuzey()
    {
        TryPurchaseCostume("kuzey", kuzeyPrice, 2);
    }
    
    private void TryPurchaseCostume(string costumeName, int price, int spriteIndex)
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager not found!");
            return;
        }
        
        // Check if costume is already unlocked
        if (CostumeShop.Instance != null && CostumeShop.Instance.IsCostumeUnlocked(costumeName))
        {
            // Costume is already owned, just select it
            ApplyCostume(spriteIndex);
            CostumeShop.Instance.SelectCostume(costumeName);
            Debug.Log($"{costumeName} costume selected!");
            return;
        }
        
        // Try to purchase the costume
        if (CurrencyManager.Instance.GetMoney() >= price)
        {
            // Successfully purchase
            bool success = CurrencyManager.Instance.SpendMoney(price);
            if (success)
            {
                // Unlock the costume
                if (CostumeShop.Instance != null)
                {
                    CostumeShop.Instance.UnlockCostume(costumeName);
                    CostumeShop.Instance.SelectCostume(costumeName);
                    CostumeShop.Instance.ShowPurchaseSuccess(costumeName);
                }
                
                // Apply the costume
                ApplyCostume(spriteIndex);
                
                // Play success animation
                PlaySuccessAnimation();
                
                // Update UI
                CurrencyManager.Instance.RefreshUI();
                
                Debug.Log($"Successfully purchased {costumeName} costume for {price} coins!");
            }
        }
        else
        {
            // Not enough money - play shake animation
            PlayShakeAnimation();
            
            // Show failure animation on button
            if (CostumeShop.Instance != null)
            {
                CostumeShop.Instance.ShowPurchaseFailure(costumeName);
            }
            
            Debug.Log($"Not enough money to buy {costumeName} costume. Need {price} coins, have {CurrencyManager.Instance.GetMoney()}");
        }
    }
    
    private void ApplyCostume(int spriteIndex)
    {
        if (playerSprite != null && customSprite.Length > spriteIndex)
        {
            playerSprite.sprite = customSprite[spriteIndex];
        }
    }
    
    /// <summary>
    /// Apply costume directly without purchase logic (for CostumeShop use)
    /// </summary>
    public void ApplyCostumeDirectly(int spriteIndex)
    {
        if (playerSprite != null && customSprite.Length > spriteIndex)
        {
            playerSprite.sprite = customSprite[spriteIndex];
        }
    }
    
    private void PlayShakeAnimation()
    {
        if (player != null)
        {
            // Use simple animation manager
            SimpleAnimationManager.Instance.SimpleShake(player.transform, shakeDuration, shakeStrength);
        }
    }
    
    private void PlaySuccessAnimation()
    {
        if (player != null)
        {
            // Use simple animation manager
            SimpleAnimationManager.Instance.SimpleScale(player.transform, successScaleMultiplier, successScaleDuration);
        }
    }
    
    /// <summary>
    /// Get the player's sprite renderer (for external access)
    /// </summary>
    public SpriteRenderer GetPlayerSprite()
    {
        return playerSprite;
    }
    
    /// <summary>
    /// Check if we have the sprite for a specific costume
    /// </summary>
    public bool HasCostumeSprite(int index)
    {
        return customSprite != null && index >= 0 && index < customSprite.Length;
    }
    
    /// <summary>
    /// Get costume price by name
    /// </summary>
    public int GetCostumePrice(string costumeName)
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
    
    private void OnDestroy()
    {
        // No cleanup needed for simple animations
    }
    
    private void OnDisable()
    {
        // No cleanup needed for simple animations
    }
}
