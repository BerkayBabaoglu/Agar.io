using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static JoystickController Instance { get; private set; }
    
    public RectTransform background;
    public RectTransform handle;
    public Vector2 InputVector { get; private set; }

    private bool isTouching = false;
    private Vector2 touchStartPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isTouching = true;
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x);
            pos.y = (pos.y / background.sizeDelta.y);

            InputVector = new Vector2(pos.x * 2, pos.y * 2);
            InputVector = (InputVector.magnitude > 1.0f) ? InputVector.normalized : InputVector;

            handle.anchoredPosition = new Vector2(InputVector.x * (background.sizeDelta.x / 2), InputVector.y * (background.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouching = true;
        touchStartPosition = eventData.position;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        InputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public Vector2 Direction()
    {
        return isTouching ? InputVector : Vector2.zero;
    }

    public bool IsTouching()
    {
        return isTouching;
    }
}