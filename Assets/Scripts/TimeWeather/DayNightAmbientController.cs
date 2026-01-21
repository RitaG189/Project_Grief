using UnityEngine;

public class DayNightAmbientController : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;

    [Header("Ambient")]
    [SerializeField] private Gradient ambientColorOverDay;
    [SerializeField] private AnimationCurve ambientIntensityOverDay;

    void Update()
    {
        float time01 = timeSystem.GetContinuousTime01();

        RenderSettings.ambientLight =
            ambientColorOverDay.Evaluate(time01);

        RenderSettings.ambientIntensity =
            ambientIntensityOverDay.Evaluate(time01);
    }

    public void ApplyPresetAmbientColor(WeatherSO weatherSO)
    {
        ambientColorOverDay = weatherSO.ambientColorOverDay;
    }
}
