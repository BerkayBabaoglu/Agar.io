using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waitTime = 2f;

    private Vector2? targetPosition;
    private Transform targetTransform;
    private float waitCounter;

    public Vector2 moveAreaMin = new Vector2(-17, -13.44f);
    public Vector2 moveAreaMax = new Vector2(17.9f, 13.35f);

    private void Start()
    {
        PickNewTarget();
    }

    private void Update()
    {
        Vector2 currentTarget;

        if (targetTransform != null && targetTransform.gameObject.activeInHierarchy)
        {
            currentTarget = targetTransform.position;
        }
        else if (targetPosition.HasValue)
        {
            currentTarget = targetPosition.Value;
        }
        else
        {
            PickNewTarget();

            if (targetPosition.HasValue)
            {
                currentTarget = targetPosition.Value;
            }
            else
            {
                return;
            }
        }

        if (Vector2.Distance(transform.position, currentTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        }
        else if (targetTransform == null)
        {
            PickNewTarget();
        }
    }

    public void PickNewTarget()
    {
        float x = Random.Range(moveAreaMin.x, moveAreaMax.x);
        float y = Random.Range(moveAreaMin.y, moveAreaMax.y);
        targetPosition = new Vector2(x, y);
        targetTransform = null;
    }

    public void SetTargetPosition(Vector2 newTarget)
    {
        targetPosition = newTarget;
        targetTransform = null;
    }

    public void SetTargetTransform(Transform newTarget)
    {
        targetTransform = newTarget;
        targetPosition = null;
    }

    public Transform GetCurrentTargetTransform()
    {
        return targetTransform;
    }
}
