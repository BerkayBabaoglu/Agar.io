using UnityEngine;
using TMPro;

/// <summary>
/// Debug script to help find parabar (1) text component
/// </summary>
public class ParabarDebug : MonoBehaviour
{
    [Header("Debug Info")]
    public TextMeshProUGUI foundParabar1Text;
    
    private void Start()
    {
        Debug.Log("=== Parabar Debug Started ===");
        FindAllParabarObjects();
    }
    
    [ContextMenu("Find All Parabar Objects")]
    public void FindAllParabarObjects()
    {
        Debug.Log("Searching for all objects with 'parabar' in name...");
        
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("parabar"))
            {
                count++;
                Debug.Log($"Found parabar object {count}: '{obj.name}' (Active: {obj.activeInHierarchy})");
                
                // Check for TextMeshProUGUI components
                TextMeshProUGUI[] texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (TextMeshProUGUI text in texts)
                {
                    Debug.Log($"  - Text component: '{text.name}' with text: '{text.text}'");
                }
                
                // If this is parabar (1), store the reference
                if (obj.name.Contains("1"))
                {
                    foundParabar1Text = obj.GetComponentInChildren<TextMeshProUGUI>();
                    if (foundParabar1Text != null)
                    {
                        Debug.Log($"  *** SET AS parabar1Text: {foundParabar1Text.name} ***");
                    }
                }
            }
        }
        
        if (count == 0)
        {
            Debug.LogWarning("No objects with 'parabar' in name found!");
        }
    }
    
    [ContextMenu("Test Set Parabar1Text")]
    public void TestSetParabar1Text()
    {
        if (foundParabar1Text != null)
        {
            foundParabar1Text.text = "TEST123";
            Debug.Log($"Set parabar1Text to: {foundParabar1Text.text}");
        }
        else
        {
            Debug.LogWarning("foundParabar1Text is null!");
        }
    }
    
    [ContextMenu("Show All TextMeshProUGUI Components")]
    public void ShowAllTextComponents()
    {
        Debug.Log("All TextMeshProUGUI components in scene:");
        
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in allTexts)
        {
            GameObject parent = text.gameObject;
            string hierarchy = "";
            while (parent != null)
            {
                hierarchy = parent.name + " > " + hierarchy;
                parent = parent.transform.parent?.gameObject;
            }
            
            Debug.Log($"Text: '{text.name}' | Parent: {hierarchy} | Text: '{text.text}'");
        }
    }
} 