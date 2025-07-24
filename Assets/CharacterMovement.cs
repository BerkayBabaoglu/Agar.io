using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance { get; private set; }

    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private float currentScale = 0.15f;
    private Vector3 targetScale;

    private bool shouldScale = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        targetScale = transform.localScale;
    }

    void Update()
    {
        if (shouldScale) {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 5f);
        
            if(Vector3.Distance(transform.localScale,targetScale)< 0.001f)
            {
                transform.localScale = targetScale;
                shouldScale = false;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            Vector2 direction = (mouseWorldPos - transform.position).normalized;
            moveDirection = direction;
        }

        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public void ScaleArtir()
    {
        currentScale += 0.01f;
        targetScale = new Vector3(currentScale, currentScale, 1f);
        shouldScale = true;
        CameraFollow.Instance.IncreaseSize(0.01f);
    }
}
