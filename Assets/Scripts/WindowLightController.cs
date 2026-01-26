using UnityEngine;

public class WindowLightController : MonoBehaviour
{
    [SerializeField] private Light[] windowLights;
    [SerializeField] private SunAndSkyController sky;
    [SerializeField] float fadeSpeed = 0.5f;

    float targetIntensity;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        // Vai buscar todas as Lights filhas (inclui inativas)
        windowLights = GetComponentsInChildren<Light>(true);
    }

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
            if (l == null) continue;

            l.intensity = Mathf.MoveTowards(
                l.intensity,
                target,
                fadeSpeed * Time.deltaTime
            );
        }
    }
}
