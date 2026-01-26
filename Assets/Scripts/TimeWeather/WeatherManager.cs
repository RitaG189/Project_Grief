using System;
using UnityEngine;
using UnityEngine.Rendering;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance {get; private set;}
    GameObject weatherPrefab;
    WeatherSO currentWeather;
    [SerializeField] LensFlareComponentSRP lensFlare;
    public static Action<WeatherSO> OnWeatherChanged;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        SunAndSkyController.OnThunder += HandleThunder;
    }

    void OnDisable()
    {
        SunAndSkyController.OnThunder -= HandleThunder;
    }

    public void ApplyWeather(WeatherSO preset)
    {
        if (weatherPrefab != null)
            Destroy(weatherPrefab);

        if (preset.rainPrefab != null)
        {
            weatherPrefab = Instantiate(preset.rainPrefab);
            AudioManager.Instance.PlayRain(preset.rainSound);
        }
        else
            AudioManager.Instance.StopRain();

        currentWeather = preset;

        if(preset.lensFlare)
            lensFlare.enabled = true;
        else
            lensFlare.enabled = false;

        OnWeatherChanged?.Invoke(preset);
    }

    public WeatherSO GetCurrentWeather()
    {
        return currentWeather;
    }

    void HandleThunder()
    {
        if (currentWeather != null && currentWeather.enableThunder)
        {
            AudioManager.Instance.PlayThunder(
                currentWeather.thunderSound
            );
        }
    }
}
