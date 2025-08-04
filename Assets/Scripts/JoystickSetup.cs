using UnityEngine;
using UnityEngine.UI;

public class JoystickSetup : MonoBehaviour
{
    [Header("Joystick UI Elementleri")]
    public JoystickController joystickController;
    public Canvas joystickCanvas;
    
    [Header("Joystick Ayarları")]
    public bool enableJoystickOnMobile = true;
    public bool enableJoystickInEditor = true;

    private void Start()
    {
        SetupJoystick();
    }

    void SetupJoystick()
    {
        if (joystickController == null)
        {
            joystickController = FindObjectOfType<JoystickController>();
        }

        if (joystickController != null)
        {
            // Joystick'i aktif et
            joystickController.gameObject.SetActive(true);
            
            // Canvas'ı doğru render mode ile ayarla
            if (joystickCanvas == null)
            {
                joystickCanvas = joystickController.GetComponentInParent<Canvas>();
            }
            
            if (joystickCanvas != null)
            {
                // Mobil için Screen Space - Overlay kullan
                joystickCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
                // Canvas'ı en üstte göster
                joystickCanvas.sortingOrder = 100;
            }
            
            Debug.Log("Joystick başarıyla kuruldu!");
        }
        else
        {
            Debug.LogWarning("JoystickController bulunamadı! Lütfen sahneye JoystickController ekleyin.");
        }
    }

    private void Update()
    {
        // Joystick durumunu kontrol et
        if (joystickController != null && joystickController.IsTouching())
        {
            Vector2 direction = joystickController.Direction();
            Debug.Log($"Joystick Yönü: {direction}");
        }
    }
} 