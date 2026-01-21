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
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyWeather(WeatherSO preset)
    {
        if (weatherPrefab != null)
            Destroy(weatherPrefab);

        if (preset.rainPrefab != null)
        {
            weatherPrefab = Instantiate(preset.rainPrefab);
            AudioManager.Instance.PlayRain(preset.rainClip);
        }
        else
            AudioManager.Instance.StopRain();

        currentWeather = preset;

        if(preset.lensFlare)
            lensFlare.enabled = true;
        else
            lensFlare.enabled = false;

        if(preset.enableThunder)
            AudioManager.Instance.PlayThunder(preset.thunderClip);

        OnWeatherChanged?.Invoke(preset);
    }

    public WeatherSO GetCurrentWeather()
    {
        return currentWeather;
    }
}
