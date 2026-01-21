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

        if (preset.weatherPrefab != null)
            weatherPrefab = Instantiate(preset.weatherPrefab);

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
}
