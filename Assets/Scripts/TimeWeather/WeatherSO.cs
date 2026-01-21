using UnityEngine;

[CreateAssetMenu(menuName = "Grief/Atmosphere Preset")]
public class WeatherSO : ScriptableObject
{
    [Header("Sky Colors")]
    public Color dayZenith;
    public Color dayHorizon;
    public Color sunsetZenith;
    public Color sunsetHorizon;
    public Color nightZenith;
    public Color nightHorizon;

    [Header("AmbientLight")]
    public Gradient ambientColorOverDay;

    [Header("Lighting")]
    public float maxLightIntensity;
    public bool lensFlare = true;
    public float windowLightsIntensity = 4;

    [Header("Exposure & Mood")]
    public float skyExposure = 1f;
    public float skySaturation = 1f;
    public float atmosphereThickness = 0.5f;

    [Header("Moon & Stars")]
    public bool moon = true;
    public bool stars = true;

    [Header("Clouds")]
    public float cloudCoverage = 0.5f;
    public float cloudSoftness = 0.5f;
    public float cloudScale = 0.05f;
    public Color dayCloudTint = Color.white;
    public Color nightCloudTint = Color.black;
    public float cloudSpeed = 0.05f;

    [Header("Rain")]
    public GameObject rainPrefab; // chuva, trovoada, etc
    public AudioClip rainClip;
    public float rainVolume = 1f;
    public bool rainLoop; 

    [Header("Thunder")]
    public bool enableThunder;
    public float thunderIntensity;
    public AudioClip thunderClip;
    public float thudnerVolume = 1f;
    public bool thunderLoop;        
}

