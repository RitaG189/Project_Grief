using UnityEngine;

public class SunAndSkyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private Light sunLight;
    [SerializeField] private Material skyboxSourceMaterial;

    private Material skyboxInstance;

    // --------------------------------------------------

    [Header("Sun Rotation")]
    [SerializeField] private float sunriseAngle = -90f;
    [SerializeField] private float sunsetAngle = 270f;

    // --------------------------------------------------

    [Header("Sun Light")]
    [SerializeField] private float maxSunIntensity = 6f;

    // --------------------------------------------------

    [Header("Sky Colors")]
    [SerializeField] private Color dayZenith = new Color(0.35f, 0.65f, 1.0f);
    [SerializeField] private Color dayHorizon = new Color(0.8f, 0.9f, 1.0f);

    [SerializeField] private Color sunsetZenith = new Color(0.55f, 0.4f, 0.3f);
    [SerializeField] private Color sunsetHorizon = new Color(1.0f, 0.6f, 0.35f);

    [SerializeField] private Color nightZenith = new Color(0.04f, 0.06f, 0.15f);
    [SerializeField] private Color nightHorizon = new Color(0.08f, 0.1f, 0.18f);

    // --------------------------------------------------

    [Header("Sky Transition Tuning")]
    [Tooltip("Quando o céu começa a clarear (mais perto de 0 = mais tarde)")]
    [SerializeField] private float skySunriseStart = -0.02f;

    [Tooltip("Quando o céu atinge dia completo")]
    [SerializeField] private float skyFullDay = 0.4f;

    [Tooltip("Largura da zona laranja no horizonte")]
    [SerializeField] private float sunsetWidth = 0.35f;

    // --------------------------------------------------

    void Awake()
    {
        // 🔴 MUITO IMPORTANTE: instanciar o material
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
        DynamicGI.UpdateEnvironment();
    }

    // --------------------------------------------------

    void UpdateAll()
    {
        UpdateSunRotation();
        UpdateSunLight();
        UpdateSkybox();
        UpdateStars();
    }

    // --------------------------------------------------
    // ☀️ ROTAÇÃO DO SOL
    // --------------------------------------------------

    void UpdateSunRotation()
    {
        float t01 = timeSystem.GetContinuousTime01();
        float angle = Mathf.Lerp(sunriseAngle, sunsetAngle, t01);
        sunLight.transform.rotation = Quaternion.Euler(angle, 170f, 0f);
    }

    // --------------------------------------------------
    // ☀️ LUZ DO SOL
    // --------------------------------------------------

    void UpdateSunLight()
    {
        // baseado na altura real do sol
        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        float intensity01 = Mathf.Clamp01(sunDot);

        sunLight.intensity = intensity01 * maxSunIntensity;

        // cor do sol (mais quente perto do horizonte)
        Color sunriseColor = new Color(1f, 0.6f, 0.4f);
        sunLight.color = Color.Lerp(sunriseColor, Color.white, intensity01);
    }

    // --------------------------------------------------
    // 🌌 SKYBOX (CÉU CONTÍNUO)
    // --------------------------------------------------

    void UpdateSkybox()
    {
        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        // transição principal noite → dia
        float t = Mathf.InverseLerp(skySunriseStart, skyFullDay, sunDot);
        t = Mathf.Clamp01(t);
        t = Mathf.SmoothStep(0f, 1f, t);

        // zona do horizonte (sunrise / sunset)
        float horizonMask = Mathf.InverseLerp(
            skySunriseStart,
            skySunriseStart + sunsetWidth,
            Mathf.Abs(sunDot)
        );
        horizonMask = 1f - Mathf.Clamp01(horizonMask);

        // base night → day
        Color zenith = Color.Lerp(nightZenith, dayZenith, t);
        Color horizon = Color.Lerp(nightHorizon, dayHorizon, t);

        // tons quentes apenas perto do horizonte
        zenith = Color.Lerp(zenith, sunsetZenith, horizonMask * (1f - t));
        horizon = Color.Lerp(horizon, sunsetHorizon, horizonMask);

        skyboxInstance.SetColor("_ZenithColor", zenith);
        skyboxInstance.SetColor("_HorizonColor", horizon);
    }

    // --------------------------------------------------
    // 🌙 ESTRELAS & LUA
    // --------------------------------------------------

    void UpdateStars()
    {
        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        // estrelas ON apenas quando o sol está abaixo do horizonte
        float stars = Mathf.InverseLerp(0.02f, -0.15f, sunDot);
        stars = Mathf.Clamp01(stars);
        stars = Mathf.SmoothStep(0f, 1f, stars);

        skyboxInstance.SetFloat("_EnableStars", stars);
        skyboxInstance.SetFloat("_EnableMoon", stars);
    }
}
