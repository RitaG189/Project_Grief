using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GriefStageManager : MonoBehaviour
{
    [SerializeField] private SunAndSkyController sky;
    [SerializeField] private WeatherManager weather;
    [SerializeField] private DayNightAmbientController ambientController;
    [SerializeField] private Volume volume;

    [Header("Presets")]
    [SerializeField] private WeatherSO denial;
    [SerializeField] private WeatherSO bargaining;
    [SerializeField] private WeatherSO anger;
    [SerializeField] private WeatherSO depression;
    [SerializeField] private WeatherSO acceptance;
    private ColorAdjustments colorAdjustments;

    void Awake()
    {
        volume.profile.TryGet(out colorAdjustments);
    }

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
        colorAdjustments.saturation.value = preset.saturation;
    }
}
