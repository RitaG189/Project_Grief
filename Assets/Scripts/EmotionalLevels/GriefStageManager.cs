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
    [SerializeField] private WeatherSO anger;
    [SerializeField] private WeatherSO bargaining;
    [SerializeField] private WeatherSO depression;
    [SerializeField] private WeatherSO acceptance;
    private ColorAdjustments colorAdjustments;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        volume.profile.TryGet(out colorAdjustments);
    }

    public void SetStage(GriefStage stage)
    {
        WeatherSO preset = stage switch
        {
            GriefStage.Denial => denial,
            GriefStage.Anger => anger,
            GriefStage.Bargaining => bargaining,
            GriefStage.Depression => depression,
            GriefStage.Acceptance => acceptance,
            _ => acceptance
        };

        Debug.Log("Weather preset: " + preset.name);

        weather.ApplyWeather(preset);

        ambientController.ApplyPresetAmbientColor(preset);

        if (colorAdjustments != null)
            colorAdjustments.saturation.value = preset.saturation;
    }
}
