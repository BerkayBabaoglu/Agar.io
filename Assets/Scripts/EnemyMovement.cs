using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waitTime = 2f;

    private Vector2 targetPosition;
    private float waitCounter;

    public Vector2 moveAreaMin = new Vector2(-17, -13.44f);
    public Vector2 moveAreaMax = new Vector2(17.9f, 13.35f);

    private void Start()
    {
        PickNewTarget();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime) {
                waitCounter = 0f;
                PickNewTarget();
            }
        }
    }

    void PickNewTarget() //haritada herhangi bir yeni konum
    {
        float x = Random.Range(moveAreaMin.x, moveAreaMax.x);
        float y = Random.Range(moveAreaMin.y, moveAreaMax.y);
        targetPosition = new Vector2(x, y);
    }


    public void SetTargetPosition(Vector2 newTarget)
    {
        targetPosition = newTarget;
        waitCounter = 0f;
    }
}
