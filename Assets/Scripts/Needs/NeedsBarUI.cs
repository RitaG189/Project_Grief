using UnityEngine;
using UnityEngine.UI;

public class NeedsBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient colorGradient;

    public void UpdateBar(float current, float max)
    {
        float value = current / max;
        slider.value = value;

        fillImage.color = colorGradient.Evaluate(value);
    }
}
