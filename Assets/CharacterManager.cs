using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Enemy enemy;
    public CharacterMovement character;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (character.IsInvulnerable())
            return;

        enemy = collision.GetComponent<Enemy>();

        if (collision.CompareTag("Enemy"))
        {
            
            if(ScoreManager.Instance.score > enemy.score)
            {
                if(enemy.score > 0)
                {
                    ScoreManager.Instance.AddScore(enemy.score);

                    float otherScale = enemy.transform.localScale.x;
                    character.ScaleArtir((float)(otherScale * 0.5));
                    enemy.gameObject.SetActive(false);

                }
                else
                {
                    ScoreManager.Instance.AddScore(0);
                    float otherScale = enemy.transform.localScale.x;
                    character.ScaleArtir(0);
                    enemy.gameObject.SetActive(false);
                }
                
                
            }
            else if(ScoreManager.Instance.score < enemy.score)
            {
                enemy.score += ScoreManager.Instance.score;
                float otherScale = enemy.transform.localScale.x;
                otherScale += character.currentScale;

                character.gameObject.SetActive(false);
            }            

        }
    }
}
