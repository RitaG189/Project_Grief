using UnityEngine;
using UnityEngine.EventSystems;

public class UIWobble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Wobble Settings")]
    public float wobbleScale = 1.05f;
    public float wobbleSpeed = 8f;
    public float wobbleRotation = 5f;

    RectTransform rect;
    bool hovering;

    Vector3 baseScale;
    Quaternion baseRotation;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseScale = rect.localScale;
        baseRotation = rect.localRotation;
    }

    void Update()
    {
        if (hovering)
        {
            float t = Time.unscaledTime * wobbleSpeed;

            float scale = 1 + Mathf.Sin(t) * (wobbleScale - 1);
            float rot = Mathf.Sin(t) * wobbleRotation;

            rect.localScale = baseScale * scale;
            rect.localRotation = Quaternion.Euler(0, 0, rot);
        }
        else
        {
            rect.localScale = Vector3.Lerp(rect.localScale, baseScale, Time.unscaledDeltaTime * 10f);
            rect.localRotation = Quaternion.Lerp(rect.localRotation, baseRotation, Time.unscaledDeltaTime * 10f);
        }
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