using UnityEngine;
using UnityEngine.UI;

public class NeedsBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateBar(float current, float max)
    {
        slider.value = current / max;
    }
}
