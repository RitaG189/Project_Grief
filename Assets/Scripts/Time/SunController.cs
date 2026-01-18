using UnityEngine;

public class SunAndSkyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private Light sunLight;
    [SerializeField] private Material skyboxSourceMaterial;

    private Material skyboxInstance;

    [Header("Sun Rotation")]
    [SerializeField] private float sunriseAngle = -90f;
    [SerializeField] private float sunsetAngle = 270f;

    [Header("Time Phases (hours)")]
    [SerializeField] private float sunriseStart = 5.5f;
    [SerializeField] private float sunriseEnd = 7.5f;

    [SerializeField] private float dayStart = 6.0f;
    [SerializeField] private float dayEnd = 16.0f;
    [SerializeField] private float sunsetEnd = 19.0f;
    [SerializeField] private float nightStart = 21.0f;

    [Header("Sky Colors")]
    [SerializeField] private Color sunriseZenith = new Color(0.75f, 0.8f, 0.85f);
    [SerializeField] private Color sunriseHorizon = new Color(1.0f, 0.85f, 0.7f);

    [SerializeField] private Color dayZenith = new Color(0.35f, 0.65f, 1.0f);
    [SerializeField] private Color dayHorizon = new Color(0.8f, 0.9f, 1.0f);

    [SerializeField] private Color sunsetZenith = new Color(0.55f, 0.4f, 0.3f);
    [SerializeField] private Color sunsetHorizon = new Color(1.0f, 0.6f, 0.35f);

    [SerializeField] private Color nightZenith = new Color(0.04f, 0.06f, 0.15f);
    [SerializeField] private Color nightHorizon = new Color(0.08f, 0.1f, 0.18f);

    void Awake()
    {
        // 🔴 CRÍTICO: instanciar o material
        skyboxInstance = new Material(skyboxSourceMaterial);
        RenderSettings.skybox = skyboxInstance;
    }

    void OnEnable()
    {
        timeSystem.OnSkippedTime += ForceUpdateSky;
    }

    void OnDisable()
    {
        timeSystem.OnSkippedTime -= ForceUpdateSky;
    }

    void Update()
    {
        UpdateAll();
    }

    public void ForceUpdateSky()
    {
        UpdateAll();
        DynamicGI.UpdateEnvironment(); // força refresh após skip
    }

    void UpdateAll()
    {
        UpdateSunRotation();
        UpdateSunLight();
        UpdateSkyColors();
        UpdateStars();
    }

    // --------------------------------------------------

    void UpdateSunRotation()
    {
        float t01 = timeSystem.GetContinuousTime01();
        float angle = Mathf.Lerp(sunriseAngle, sunsetAngle, t01);
        sunLight.transform.rotation = Quaternion.Euler(angle, 170f, 0f);
    }

    // --------------------------------------------------

    void UpdateSunLight()
    {
        float hour = timeSystem.Hour + timeSystem.Minute / 60f;

        // INTENSITY
        if (hour < sunriseStart || hour > sunsetEnd)
            sunLight.intensity = 0f;
        else
        {
            float t = Mathf.InverseLerp(sunriseStart, sunsetEnd, hour);
            sunLight.intensity = Mathf.Sin(t * Mathf.PI);
        }

        // COLOR
        if (hour >= sunriseStart && hour <= sunriseEnd)
        {
            float t = Mathf.InverseLerp(sunriseStart, sunriseEnd, hour);
            sunLight.color = Color.Lerp(
                new Color(1f, 0.85f, 0.7f),
                Color.white,
                t
            );
        }
        else if (hour >= dayEnd && hour <= sunsetEnd)
        {
            float t = Mathf.InverseLerp(dayEnd, sunsetEnd, hour);
            sunLight.color = Color.Lerp(
                Color.white,
                new Color(1f, 0.55f, 0.35f),
                t
            );
        }
        else
        {
            sunLight.color = Color.white;
        }
    }

    // --------------------------------------------------

    void UpdateSkyColors()
    {
        float hour = timeSystem.Hour + timeSystem.Minute / 60f;

        Color zenith;
        Color horizon;

        if (hour < sunriseStart)
        {
            zenith = nightZenith;
            horizon = nightHorizon;
        }
        else if (hour < sunriseEnd)
        {
            float t = Mathf.InverseLerp(sunriseStart, sunriseEnd, hour);
            zenith = Color.Lerp(nightZenith, sunriseZenith, t);
            horizon = Color.Lerp(nightHorizon, sunriseHorizon, t);
        }
        else if (hour < dayEnd)
        {
            zenith = dayZenith;
            horizon = dayHorizon;
        }
        else if (hour < sunsetEnd)
        {
            float t = Mathf.InverseLerp(dayEnd, sunsetEnd, hour);
            zenith = Color.Lerp(dayZenith, sunsetZenith, t);
            horizon = Color.Lerp(dayHorizon, sunsetHorizon, t);
        }
        else if (hour < nightStart)
        {
            float t = Mathf.InverseLerp(sunsetEnd, nightStart, hour);
            zenith = Color.Lerp(sunsetZenith, nightZenith, t);
            horizon = Color.Lerp(sunsetHorizon, nightHorizon, t);
        }
        else
        {
            zenith = nightZenith;
            horizon = nightHorizon;
        }

        skyboxInstance.SetColor("_ZenithColor", zenith);
        skyboxInstance.SetColor("_HorizonColor", horizon);
    }

    // --------------------------------------------------

    void UpdateStars()
    {
        float hour = timeSystem.Hour + timeSystem.Minute / 60f;
        float stars;

        // 🌞 Dia (sem estrelas)
        if (hour >= dayStart && hour < sunsetEnd)
        {
            stars = 0f;
        }
        // 🌆 Sunset → Night (fade in)
        else if (hour >= sunsetEnd && hour < nightStart)
        {
            float t = Mathf.InverseLerp(sunsetEnd, nightStart, hour);
            stars = Mathf.SmoothStep(0f, 1f, t);
        }
        // 🌙 Noite (inclui meia-noite até sunrise)
        else
        {
            stars = 1f;
        }

        skyboxInstance.SetFloat("_EnableStars", stars);
        skyboxInstance.SetFloat("_EnableMoon", stars);
    }
}
