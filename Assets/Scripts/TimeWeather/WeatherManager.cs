using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance {get; private set;}
    GameObject currentWeather;

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
        if (currentWeather != null)
            Destroy(currentWeather);

        if (preset.weatherPrefab != null)
            currentWeather = Instantiate(preset.weatherPrefab);
    }
}
