using Unity.VisualScripting;
using UnityEngine;

public class WindowLightController : MonoBehaviour
{
    [SerializeField] private Light[] windowLights;
    [SerializeField] private SunAndSkyController sky;
    [SerializeField] float fadeSpeed = 0.5f;

    float targetIntensity;

    void OnEnable()
    {
        WeatherManager.OnWeatherChanged += OnWeatherChanged;
    }

    void OnDisable()
    {
        WeatherManager.OnWeatherChanged -= OnWeatherChanged;
    }

    void OnWeatherChanged(WeatherSO weather)
    {
        targetIntensity = weather.windowLightsIntensity;
    }

    void Update()
    {
        float target = sky.IsDay ? targetIntensity : 0f;

        foreach (var l in windowLights)
        {
            l.intensity = Mathf.MoveTowards(
                l.intensity,
                target,
                fadeSpeed * Time.deltaTime
            );
        }
    }
}
