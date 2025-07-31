using UnityEngine;

public class Food : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            CharacterMovement.Instance.ScaleArtir();
            ScoreManager.Instance.AddScore(1);
        }
        else if (collision.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.AddScore(1);
            }
        }
    }

}