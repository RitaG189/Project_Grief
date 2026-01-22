using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SunAndSkyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private Light sunLight;
    [SerializeField] private Material skyboxSourceMaterial;
    public static event Action OnThunder;
    private Material skyboxInstance;

    // --------------------------------------------------
    [Header("Sun Rotation")]
    [SerializeField] private float sunriseAngle = -90f;
    [SerializeField] private float sunsetAngle = 270f;

    // --------------------------------------------------
    [Header("Sun Light")]
    [SerializeField] private float maxSunIntensity = 6f;
    public float SunHeight01 { get; private set; }
    public bool IsDay => SunHeight01 > 0.3f;

    // --------------------------------------------------
    [Header("Sky Colors")]
    [SerializeField] private Color dayZenith = new(0.35f, 0.65f, 1.0f);
    [SerializeField] private Color dayHorizon = new(0.8f, 0.9f, 1.0f);

    [SerializeField] private Color sunsetZenith = new(0.55f, 0.4f, 0.3f);
    [SerializeField] private Color sunsetHorizon = new(1.0f, 0.6f, 0.35f);

    [SerializeField] private Color nightZenith = new(0.04f, 0.06f, 0.15f);
    [SerializeField] private Color nightHorizon = new(0.08f, 0.1f, 0.18f);

    // --------------------------------------------------
    [Header("Sky Settings")]
    [SerializeField] private float cloudCoverage = 1f;
    [SerializeField] private float cloudScale = 1f;
    [SerializeField] private Color dayCloudTint = Color.white;
    [SerializeField] private Color nightCloudTint = new(0.15f, 0.15f, 0.2f);
    [SerializeField] private bool stars;
    [SerializeField] private bool moon;

    // --------------------------------------------------
    [Header("Thunder")]
    [SerializeField] private Light thunderLight;
    [SerializeField] private float thunderIntensity = 2.5f;
    [SerializeField] private float thunderMinDelay = 6f;
    [SerializeField] private float thunderMaxDelay = 18f;

    private Coroutine thunderRoutine;

    // --------------------------------------------------
    [Header("Sky Transition Tuning")]
    [SerializeField] private float skySunriseStart = -0.02f;
    [SerializeField] private float skyFullDay = 0.4f;
    [SerializeField] private float sunsetWidth = 0.35f;

    // --------------------------------------------------

    void Awake()
    {
        skyboxInstance = new Material(skyboxSourceMaterial);
        RenderSettings.skybox = skyboxInstance;

        if (thunderLight != null)
            thunderLight.intensity = 0f;
    }

    void OnEnable()
    {
        timeSystem.OnSkippedTime += ForceUpdateSky;
    }

    void OnDisable()
    {
        timeSystem.OnSkippedTime -= ForceUpdateSky;
        CancelInvoke(nameof(RandomThunder));
    }

    void Update()
    {
        UpdateAll();
    }

    public void ForceUpdateSky()
    {
        UpdateAll();
        DynamicGI.UpdateEnvironment();
    }

    // --------------------------------------------------

    void UpdateAll()
    {
        UpdateSunRotation();
        UpdateSunLight();
        UpdateSkybox();
        UpdateStarsAndMoon();
    }

    // --------------------------------------------------
    // ☀️ SUN ROTATION
    // --------------------------------------------------

    void UpdateSunRotation()
    {
        float t01 = timeSystem.GetContinuousTime01();
        float angle = Mathf.Lerp(sunriseAngle, sunsetAngle, t01);
        sunLight.transform.rotation = Quaternion.Euler(angle, 200f, 0f);
    }

    // --------------------------------------------------
    // ☀️ SUN LIGHT
    // --------------------------------------------------

    void UpdateSunLight()
    {
        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Clamp01(sunDot) * maxSunIntensity;

        SunHeight01 = Mathf.Clamp01(
            Vector3.Dot(sunLight.transform.forward, Vector3.down)
        );
    }

    // --------------------------------------------------
    // 🌌 SKYBOX
    // --------------------------------------------------

    void UpdateSkybox()
    {
        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        float t = Mathf.InverseLerp(skySunriseStart, skyFullDay, sunDot);
        t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));

        float horizonMask = Mathf.InverseLerp(
            skySunriseStart,
            skySunriseStart + sunsetWidth,
            Mathf.Abs(sunDot)
        );
        horizonMask = 1f - Mathf.Clamp01(horizonMask);

        Color zenith = Color.Lerp(nightZenith, dayZenith, t);
        Color horizon = Color.Lerp(nightHorizon, dayHorizon, t);

        zenith = Color.Lerp(zenith, sunsetZenith, horizonMask * (1f - t));
        horizon = Color.Lerp(horizon, sunsetHorizon, horizonMask);

        Color clouds = Color.Lerp(nightCloudTint, dayCloudTint, t);

        skyboxInstance.SetColor("_ZenithColor", zenith);
        skyboxInstance.SetColor("_HorizonColor", horizon);
        skyboxInstance.SetColor("_CloudTint", clouds);
        skyboxInstance.SetFloat("_CloudCoverage", cloudCoverage);
        skyboxInstance.SetFloat("_CloudScale", cloudScale);
    }

    // --------------------------------------------------
    // 🌙 STARS & MOON
    // --------------------------------------------------

    void UpdateStarsAndMoon()
    {
        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        if (stars)
        {
            float s = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(
                Mathf.InverseLerp(0.02f, -0.15f, sunDot)));
            skyboxInstance.SetFloat("_EnableStars", s);
        }

        if (moon)
        {
            float m = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(
                Mathf.InverseLerp(0.25f, -0.35f, sunDot)));
            skyboxInstance.SetFloat("_EnableMoon", m);
        }
    }

    // --------------------------------------------------
    // 🌩️ THUNDER SYSTEM
    // --------------------------------------------------

    void ScheduleThunder()
    {
        Invoke(nameof(RandomThunder), UnityEngine.Random.Range(thunderMinDelay, thunderMaxDelay));
    }

    void RandomThunder()
    {
        TriggerThunder();
        ScheduleThunder();
    }

    void TriggerThunder()
    {
        if (thunderLight == null) return;

        OnThunder?.Invoke();

        if (thunderRoutine != null)
            StopCoroutine(thunderRoutine);

        thunderRoutine = StartCoroutine(ThunderFlash());
    }

    IEnumerator ThunderFlash()
    {
        Color baseHorizon = skyboxInstance.GetColor("_HorizonColor");
        Color baseClouds = skyboxInstance.GetColor("_CloudTint");

        // FLASH 1
        thunderLight.intensity = thunderIntensity;
        skyboxInstance.SetColor("_HorizonColor", Color.white);
        skyboxInstance.SetColor("_CloudTint", Color.Lerp(baseClouds, Color.white, 0.65f));

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.04f, 0.09f));

        
        skyboxInstance.SetColor("_HorizonColor", baseHorizon);
        skyboxInstance.SetColor("_CloudTint", baseClouds);
        thunderLight.intensity = 0f;

        yield return new WaitForSeconds(1f);

        
    }

    // --------------------------------------------------
    // 🌦️ WEATHER PRESET
    // --------------------------------------------------

    public void ApplyAtmospherePreset(WeatherSO weatherSO)
    {
        dayZenith = weatherSO.dayZenith;
        dayHorizon = weatherSO.dayHorizon;
        sunsetZenith = weatherSO.sunsetZenith;
        sunsetHorizon = weatherSO.sunsetHorizon;
        nightZenith = weatherSO.nightZenith;
        nightHorizon = weatherSO.nightHorizon;

        cloudCoverage = weatherSO.cloudCoverage;
        cloudScale = weatherSO.cloudScale;
        dayCloudTint = weatherSO.dayCloudTint;
        nightCloudTint = weatherSO.nightCloudTint;

        maxSunIntensity = weatherSO.maxLightIntensity;
        stars = weatherSO.stars;
        moon = weatherSO.moon;

        CancelInvoke(nameof(RandomThunder));

        if (weatherSO.enableThunder)
        {
            thunderIntensity = weatherSO.thunderIntensity;
            ScheduleThunder();
        }
    }
}
