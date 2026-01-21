using UnityEngine;

public class GriefStageManager : MonoBehaviour
{
    [SerializeField] private SunAndSkyController sky;
    [SerializeField] private WeatherManager weather;
    [SerializeField] private DayNightAmbientController ambientController;

    [Header("Presets")]
    [SerializeField] private WeatherSO denial;
    [SerializeField] private WeatherSO bargaining;
    [SerializeField] private WeatherSO anger;
    [SerializeField] private WeatherSO depression;
    [SerializeField] private WeatherSO acceptance;

    public void SetStage(GriefStage stage)
    {
        WeatherSO preset = stage switch
        {
            GriefStage.Denial => denial,
            GriefStage.Bargaining => bargaining,
            GriefStage.Anger => anger,
            GriefStage.Depression => depression,
            GriefStage.Acceptance => acceptance,
            _ => acceptance
        };

        print(preset.name);

        sky.ApplyAtmospherePreset(preset);
        ambientController.ApplyPresetAmbientColor(preset);
        weather.ApplyWeather(preset);
    }
}
