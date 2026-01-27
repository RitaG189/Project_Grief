using UnityEngine;
using UnityEngine.UI;

public class NeedsBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    [SerializeField] private Image iconUp;
    [SerializeField] private Image iconDown;

    [SerializeField] private Gradient colorGradient;

    private float currentValue01;

    public void UpdateBar(float current, float max)
    {
        currentValue01 = Mathf.Clamp01(current / max);

        slider.value = currentValue01;
        fillImage.color = colorGradient.Evaluate(currentValue01);
    }

    /// <summary>
    /// Usa diretamente o valor recebido:
    /// positivo = sobe | negativo = desce
    /// </summary>
    public void ShowChange(float value)
    {
        if (value > 0f)
        {
            iconUp.gameObject.SetActive(true);
            iconDown.gameObject.SetActive(false);
        }
        else if (value < 0f)
        {
            iconUp.gameObject.SetActive(false);
            iconDown.gameObject.SetActive(true);
        }
        else
        {
            iconUp.gameObject.SetActive(false);
            iconDown.gameObject.SetActive(true);
        }
    }

    public void ShowEmotionalEnergyChange(float reward, float cost)
    {
        if (reward > 0f)
        {
            iconUp.gameObject.SetActive(true);
            iconDown.gameObject.SetActive(false);
        }
        else if(cost > 0)
        {
            iconUp.gameObject.SetActive(false);
            iconDown.gameObject.SetActive(true);
        }
        else
        {
            iconUp.gameObject.SetActive(false);
            iconDown.gameObject.SetActive(false);
        }
    }

    public void ClearIcons()
    {
        if (iconUp != null)
            iconUp.gameObject.SetActive(false);

        if (iconDown != null)
            iconDown.gameObject.SetActive(false);
    }
}