using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance { get; private set; }

    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public float currentScale = 0.15f;
    private Vector3 targetScale;

    private bool shouldScale = false;

    // Ölümsüzlük sistemi
    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 3f;
    private float invulnerabilityTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        targetScale = transform.localScale;
        

        StartInvulnerability(); //karakter dogdugunda olumsuz baslat 3sn
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


        if (isInvulnerable) //olumsuzluk
        {
            invulnerabilityTimer -= Time.deltaTime;
            

            if (spriteRenderer != null) //yanıp sonme efekti
            {
                float alpha = Mathf.PingPong(Time.time * 5f, 1f);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            
            if (invulnerabilityTimer <= 0f)
            {
                EndInvulnerability();
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

    public void ScaleArtir(float amount)
    {
        currentScale += amount;
        targetScale = new Vector3(currentScale, currentScale, 1f);
        shouldScale = true;
        CameraFollow.Instance.IncreaseSize(amount);
    }


    public void StartInvulnerability() //olumsuzluk baslat
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }


    public void EndInvulnerability() //olumsuzluk bitir
    {
        isInvulnerable = false;
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }


    public bool IsInvulnerable() //durum kontrol
    {
        return isInvulnerable;
    }

    // Oyuncu öldüğünde çağrılacak metod
    public void PlayerDied()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.PlayerDied(ScoreManager.Instance.playerName);
        }
    }
}
