using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public int score = 0;
    public float growFactor = 0.01f; //yedikce buyume orani
    public float growSpeed = 5f;

    private float currentScale = 0.34f;
    private Vector2 targetScale;

    private bool shouldScale = false;


    private void Start()
    {
        targetScale = transform.localScale;
    }

    private void Update()
    {
        if (shouldScale)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * 5f);

        }
    }


    public void SetName(string newName)
    {
        if (nameText != null)
        {  
            nameText.text = newName;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        ScaleArtir();
    }

    void ScaleArtir()
    {
        currentScale += 0.01f;
        targetScale = new Vector3 (currentScale, currentScale, 1);
        shouldScale = true;
    }
}
