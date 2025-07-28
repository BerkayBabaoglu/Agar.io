using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float foodDetectionRadius = 8f;
    private Enemy enemy;
    private EnemyMovement movement;
    
    private float lastTargetUpdate = 0f;
    private float targetUpdateInterval = 0.5f;

    private Enemy currentChaseTarget;
    private float chaseStartTime;
    private float chaseDuration = 3f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        movement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if(currentChaseTarget != null)
        {
            float dist = Vector2.Distance(transform.position, currentChaseTarget.transform.position);

            if(Time.time  - chaseStartTime < chaseDuration)
            {
                movement.SetTargetPosition(currentChaseTarget.transform.position);
                return;
            }
            else
            {
                currentChaseTarget = null;
                movement.PickNewTarget();
            }
        }

        if (Time.time - lastTargetUpdate > targetUpdateInterval)
        {
            lastTargetUpdate = Time.time;
            UpdateTarget();
        }
    }

    private void UpdateTarget()
    {
        Enemy closestEnemy = FindClosestEnemy();
        
        if (closestEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, closestEnemy.transform.position);

            if (closestEnemy.score < enemy.score)
            {
                currentChaseTarget = closestEnemy;
                chaseStartTime = Time.time;
                movement.SetTargetTransform(closestEnemy.transform);
                return;
            }
            else if (closestEnemy.score > enemy.score && distance < detectionRadius)
            {
                Vector2 fleeDirection = (transform.position - closestEnemy.transform.position).normalized;
                Vector2 fleeTarget = (Vector2)transform.position + fleeDirection * 3f;
                movement.SetTargetPosition(fleeTarget);
                return;
            }
        }

        GameObject closestFood = FindClosestFood();
        if (closestFood != null)
        {
            movement.SetTargetPosition(closestFood.transform.position);
        }
        else
        {
            movement.PickNewTarget();
        }
    }

    Enemy FindClosestEnemy()
    {
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy e in allEnemies)
        {
            if (e == enemy || !e.gameObject.activeInHierarchy) continue;

            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDistance && dist < detectionRadius)
            {
                minDistance = dist;
                closest = e;
            }
        }
        return closest;
    }

    GameObject FindClosestFood()
    {
        GameObject[] allFoods = GameObject.FindGameObjectsWithTag("Food");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject food in allFoods)
        {
            if (!food.activeInHierarchy) continue;

            float dist = Vector2.Distance(transform.position, food.transform.position);
            if (dist < minDistance && dist < foodDetectionRadius)
            {
                minDistance = dist;
                closest = food;
            }
        }
        return closest;
    }
}