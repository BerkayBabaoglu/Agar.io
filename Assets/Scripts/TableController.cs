using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TableController : MonoBehaviour
{
    public RectTransform tablePanel;
    public CanvasGroup canvasGroup;
    public float duration = 0.5f;
    public Vector2 panelOffset = new Vector2(800f, 0f); // panel kapal� konumdayken offset

    private bool isOpen = false;
    private Vector2 openPosition;
    private Vector2 closedPosition;

    [Header("Optional: A�ma/Kapama butonu")]
    public RectTransform toggleButton; // Bu butonun konumuna g�re panel sa�a m� sola m� a��lacak, karar verilir

    void Start()
    {
        // E�er toggleButton atanm��sa ona g�re panelin y�n�n� belirle
        if (toggleButton != null)
        {
            bool isButtonOnRight = toggleButton.position.x > Screen.width / 2f;
            openPosition = Vector2.zero;

            closedPosition = isButtonOnRight ? panelOffset : -panelOffset;
            tablePanel.anchoredPosition = closedPosition;
        }
        else
        {
            openPosition = Vector2.zero;
            closedPosition = panelOffset;
            tablePanel.anchoredPosition = closedPosition;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ToggleTable()
    {
        if (isOpen)
        {
            tablePanel.DOAnchorPos(closedPosition, duration).SetEase(Ease.OutQuad);
            canvasGroup.DOFade(0, duration * 0.8f);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            tablePanel.DOAnchorPos(openPosition, duration).SetEase(Ease.OutQuad);
            canvasGroup.DOFade(1, duration);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        isOpen = !isOpen;
    }
}
