using System.Runtime.CompilerServices;
using UnityEngine;


[RequireComponent(typeof(EnemyMovement), typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
    public float detectionRadius = 5f;
    private Enemy enemy;
    private EnemyMovement movement;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        movement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        Enemy target = FindClosestEnemy();

        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (target.score < enemy.score)
            {
                movement.SetTargetPosition(target.transform.position);
            }
            else if (target.score > enemy.score && distance < detectionRadius)
            {
                Vector2 fleeDirection = (transform.position - target.transform.position).normalized;
                Vector2 fleeTarget = (Vector2)transform.position + fleeDirection * 3f;
                movement.SetTargetPosition(fleeTarget);
            }

        }
    }

    Enemy FindClosestEnemy()
    {
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy e in allEnemies)
        {
            if (e == enemy) continue;

            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = e;
            }
        }
        return closest;
    }

}