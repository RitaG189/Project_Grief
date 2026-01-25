using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class NeedsEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;

    [Header("Hygiene → Bloom Dirt")]
    [SerializeField] private float hygieneThreshold = 10f;
    [SerializeField] private float dirtyAtZero = 1f;
    [SerializeField] private float maxCriticalDirt = 1000f;

    [Header("Energy → Vignette")]
    [SerializeField] private float energyThreshold = 10f;
    [SerializeField] private float vignetteAtZero = 0.45f;
    [SerializeField] private float maxCriticalVignette = 1f;

    [Header("Hunger → Chromatic Aberration")]
    [SerializeField] private float hungerThreshold = 10f;
    [SerializeField] private float chromaticAtZero = 0.45f;
    [SerializeField] private float maxCriticalChromatic = 1f;

    [Header("Hunger → Depth Of Field")]
    [SerializeField] private float dofBlurAtZero = 0.3f;
    [SerializeField] private float maxCriticalDOF = 1f;


    [Header("Common")]
    [SerializeField] private float lerpSpeed = 1.5f;

    private Bloom bloom;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    private float targetDirtIntensity;
    private float targetVignetteIntensity;
    private float targetChromaticIntensity;

    void Awake()
    {
        globalVolume.profile = Instantiate(globalVolume.profile);

        if (!globalVolume.profile.TryGet(out bloom))
            Debug.LogError("Bloom não encontrado no Volume");

        if (!globalVolume.profile.TryGet(out vignette))
            Debug.LogError("Vignette não encontrado no Volume");

        if (!globalVolume.profile.TryGet(out chromaticAberration))
            Debug.LogError("Vignette não encontrado no Volume");

        // reset garantido
        bloom.dirtIntensity.value = 0f;
        vignette.intensity.value = 0f;
        chromaticAberration.intensity.value = 0f;
    }

    void OnEnable()
    {
        StartCoroutine(SubscribeWhenReady());
    }

    IEnumerator SubscribeWhenReady()
    {
        yield return new WaitUntil(() => NeedsManager.Instance != null);

        var manager = NeedsManager.Instance;

        manager.OnHygieneChanged += OnHygieneChanged;
        manager.OnEnergyChanged += OnEnergyChanged;
        manager.OnHungerChanged += OnHungerChanged;

        // força estado inicial
        OnHygieneChanged(manager.Needs.Hygiene, manager.Needs.MaxHygiene);
        OnEnergyChanged(manager.Needs.Energy, manager.Needs.MaxEnergy);
        OnHungerChanged(manager.Needs.Hunger, manager.Needs.MaxHunger);
    }

    void OnDisable()
    {
        if (NeedsManager.Instance == null) return;

        NeedsManager.Instance.OnHygieneChanged -= OnHygieneChanged;
        NeedsManager.Instance.OnEnergyChanged -= OnEnergyChanged;
        NeedsManager.Instance.OnHungerChanged -= OnHungerChanged;
    }

    void Update()
    {
        if (bloom != null)
        {
            bloom.dirtIntensity.value = Mathf.Lerp(
                bloom.dirtIntensity.value,
                targetDirtIntensity,
                Time.deltaTime * lerpSpeed
            );
        }

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(
                vignette.intensity.value,
                targetVignetteIntensity,
                Time.deltaTime * lerpSpeed
            );
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(
                chromaticAberration.intensity.value,
                targetChromaticIntensity,
                Time.deltaTime * lerpSpeed
            );
        }
    }

    // ───────────── Hygiene ─────────────
    void OnHygieneChanged(float current, float max)
    {
        var manager = NeedsManager.Instance;

        if (current > hygieneThreshold)
        {
            targetDirtIntensity = 0f;
            bloom.dirtIntensity.value = 0f; // hard reset
            return;
        }

        if (current > 0f)
        {
            float t = Mathf.InverseLerp(hygieneThreshold, 0f, current);
            targetDirtIntensity = Mathf.Lerp(0f, dirtyAtZero, t);
            return;
        }

        float zero01 = Mathf.Clamp01(
            manager.HygieneZeroHours / manager.HoursUntilGameOver
        );

        targetDirtIntensity = Mathf.Lerp(
            dirtyAtZero,
            maxCriticalDirt,
            zero01
        );
    }

    // ───────────── Energy ─────────────
    void OnEnergyChanged(float current, float max)
    {
        var manager = NeedsManager.Instance;

        if (current > energyThreshold)
        {
            targetVignetteIntensity = 0f;
            vignette.intensity.value = 0f; // hard reset
            return;
        }

        if (current > 0f)
        {
            float t = Mathf.InverseLerp(energyThreshold, 0f, current);
            targetVignetteIntensity = Mathf.Lerp(0f, vignetteAtZero, t);
            return;
        }

        float zero01 = Mathf.Clamp01(
            manager.EnergyZeroHours / manager.HoursUntilGameOver
        );

        targetVignetteIntensity = Mathf.Lerp(
            vignetteAtZero,
            maxCriticalVignette,
            zero01
        );
    }

    // ───────────── Hunger ─────────────

    void OnHungerChanged(float current, float max)
    {
        var manager = NeedsManager.Instance;

        if (current > hungerThreshold)
        {
            targetChromaticIntensity = 0f;
            chromaticAberration.intensity.value = 0f; // hard reset
            return;
        }

        if (current > 0f)
        {
            float t = Mathf.InverseLerp(hungerThreshold, 0f, current);
            targetChromaticIntensity = Mathf.Lerp(0f, chromaticAtZero, t);
            return;
        }

        float zero01 = Mathf.Clamp01(
            manager.HungerZeroHours / manager.HoursUntilGameOver
        );

        targetChromaticIntensity = Mathf.Lerp(
            chromaticAtZero,
            maxCriticalChromatic,
            zero01
        );
    }
}
