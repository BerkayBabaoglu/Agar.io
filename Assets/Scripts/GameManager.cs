using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Game;
    public GameObject Menu;

    public CharacterMovement character;

    private void Awake()
    {
        Instance = this;
    }

    public void GameLoad()
    {
        Game.SetActive(true);
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.isReloading = false;
    }
    
    public void MenuLoad()
    {
        Menu.SetActive(true);
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.isReloading = false;


        if (Menu.gameObject.activeSelf == true)
        {
            character.gameObject.SetActive(true);
            

            ScoreManager.Instance.score = 0;
            ScoreManager.Instance.UpdateScoreText();
            

            character.currentScale = 0.15f;
            character.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
            

            if (CameraFollow.Instance != null)
            {
                CameraFollow.Instance.m_Camera.orthographicSize = 2f;
                CameraFollow.Instance.targetSize = 2f;
            }
            
            character.StartInvulnerability();

            Debug.Log("Menu Load");
        }
    }

    public void GameDeactive()
    {

        Color color = CameraFollow.Instance.panel.color;
        color.a = 0f;
        CameraFollow.Instance.panel.color = color;

        Game.SetActive(false);

        Debug.Log("GameDeactive");

    }

    public void MenuDeactive() { 
        Game.SetActive(false);
    }
}
