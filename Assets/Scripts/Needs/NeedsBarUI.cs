using UnityEngine;
using UnityEngine.UI;

public class NeedsBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image previewFillImage;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private Color positivePreviewColor = new Color(0f, 1f, 0f, 0.4f);
    [SerializeField] private Color negativePreviewColor = new Color(1f, 0f, 0f, 0.4f);

    float currentValue01;

    public void UpdateBar(float current, float max)
    {
        currentValue01 = current / max;
        slider.value = currentValue01;

        fillImage.color = colorGradient.Evaluate(currentValue01);
        ClearPreview();
    }

    public void ShowPreview(float delta, float max)
    {
        float previewValue = Mathf.Clamp01((currentValue01 * max + delta) / max);

        previewFillImage.fillAmount = previewValue;

        bool isPositive = delta > 0;
        previewFillImage.color = isPositive ? positivePreviewColor : negativePreviewColor;

        previewFillImage.gameObject.SetActive(true);
    }

    public void ClearPreview()
    {
        previewFillImage.gameObject.SetActive(false);
    }
}
