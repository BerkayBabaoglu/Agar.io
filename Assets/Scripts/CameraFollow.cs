using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    private Camera m_Camera;
    private float targetSize;

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Awake()
    {
        Instance = this;
        m_Camera = GetComponent<Camera>();
    }

    void Start()
    {
        targetSize = m_Camera.orthographicSize;
    }

    void LateUpdate()
    {
        // Kamera pozisyon takibi (opsiyonel)
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition.z = -10f; // 2D için sabit Z
            transform.position = smoothedPosition;
        }

        // Kamera büyüklüðünü yumuþat
        m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, targetSize, Time.deltaTime * 5f);
    }

    public void IncreaseSize(float amount)
    {
        targetSize += amount;
    }

    public Camera GetCamera()
    {
        return m_Camera;
    }
}
