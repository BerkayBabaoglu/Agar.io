using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    public Camera m_Camera;
    public float targetSize;

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public Image panel;
    private float fadeSpeed = 1.0f;

    public bool isReloading = false;

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
        if (target != null && target.gameObject.activeInHierarchy)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition.z = -10f;
            transform.position = smoothedPosition;

            m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, targetSize, Time.deltaTime * 5f);
        }
        else if (!isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadScene());
        }
    }

    public void IncreaseSize(float amount)
    {
        targetSize += amount;
    }

    public Camera GetCamera()
    {
        return m_Camera;
    }

    public IEnumerator FadeImage()
    {
        Color color = panel.color;
        color.a = 0f;
        panel.color = color;

        float elapsedTime = 0f;

        while(elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeSpeed);
            panel.color = color;
            yield return null;
        }

    }


    public IEnumerator ReloadScene()
    {
        yield return StartCoroutine(FadeImage());

        yield return new WaitForSeconds(0.2f);

        GameManager.Instance.GameDeactive();

        GameManager.Instance.MenuLoad();

        Debug.Log("ReloadScene");
    }
}
