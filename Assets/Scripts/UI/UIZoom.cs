using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Zoom Settings")]
    [SerializeField] float hoverScale = 1.08f;
    [SerializeField] float speed = 12f;

    RectTransform rect;
    Vector3 baseScale;
    bool hovering;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseScale = rect.localScale;
    }

    void Update()
    {
        Vector3 targetScale = hovering ? baseScale * hoverScale : baseScale;
        rect.localScale = Vector3.Lerp(
            rect.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}