using UnityEngine;
using System.Collections;

/// <summary>
/// Simple animation manager without DOTween to avoid memory leaks
/// </summary>
public class SimpleAnimationManager : MonoBehaviour
{
    private static SimpleAnimationManager instance;
    public static SimpleAnimationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SimpleAnimationManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SimpleAnimationManager");
                    instance = go.AddComponent<SimpleAnimationManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Simple shake animation using coroutines
    /// </summary>
    public void SimpleShake(Transform target, float duration = 0.5f, float strength = 0.1f)
    {
        if (target == null) return;
        
        StartCoroutine(SimpleShakeCoroutine(target, duration, strength));
    }
    
    private IEnumerator SimpleShakeCoroutine(Transform target, float duration, float strength)
    {
        if (target == null) yield break;
        
        Vector3 originalPosition = target.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            if (target == null) yield break;
            
            float x = originalPosition.x + Random.Range(-strength, strength);
            float y = originalPosition.y + Random.Range(-strength, strength);
            target.position = new Vector3(x, y, originalPosition.z);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (target != null)
        {
            target.position = originalPosition;
        }
    }
    
    /// <summary>
    /// Simple scale animation using coroutines
    /// </summary>
    public void SimpleScale(Transform target, float scaleMultiplier = 1.2f, float duration = 0.3f)
    {
        if (target == null) return;
        
        StartCoroutine(SimpleScaleCoroutine(target, scaleMultiplier, duration));
    }
    
    private IEnumerator SimpleScaleCoroutine(Transform target, float scaleMultiplier, float duration)
    {
        if (target == null) yield break;
        
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * scaleMultiplier;
        float elapsed = 0f;
        
        // Scale up
        while (elapsed < duration)
        {
            if (target == null) yield break;
            
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(originalScale, targetScale, t);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (target != null)
        {
            target.localScale = targetScale;
        }
        
        // Scale down
        elapsed = 0f;
        while (elapsed < duration * 0.5f)
        {
            if (target == null) yield break;
            
            float t = elapsed / (duration * 0.5f);
            target.localScale = Vector3.Lerp(targetScale, originalScale, t);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (target != null)
        {
            target.localScale = originalScale;
        }
    }
    
    /// <summary>
    /// Simple color flash animation
    /// </summary>
    public void SimpleColorFlash(UnityEngine.UI.Image image, Color flashColor, float duration = 0.2f)
    {
        if (image == null) return;
        
        StartCoroutine(SimpleColorFlashCoroutine(image, flashColor, duration));
    }
    
    private IEnumerator SimpleColorFlashCoroutine(UnityEngine.UI.Image image, Color flashColor, float duration)
    {
        if (image == null) yield break;
        
        Color originalColor = image.color;
        image.color = flashColor;
        
        yield return new WaitForSeconds(duration);
        
        if (image != null)
        {
            image.color = originalColor;
        }
    }
} 