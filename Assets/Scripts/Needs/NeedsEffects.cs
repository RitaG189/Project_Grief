using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class NeedsEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;

    [Header("Hygiene → Lens Dirt Overlay")]
    [SerializeField] private Renderer[] lensDirtRenderers;
    [SerializeField] private float hygieneThreshold = 10f;
    [SerializeField] private float maxOverlayAlpha = 0.25f;
    [SerializeField] private float maxCriticalOverlayAlpha = 0.6f;

    [Header("Energy → Vignette Pulse")]
    [SerializeField] private float pulseAtZero = 0.3f;
    [SerializeField] private float maxPulseIntensity = 1f;
    [SerializeField] private float pulsePeriodAtZero = 5f;
    [SerializeField] private float pulsePeriodAtCritical = 0.4f;

    [Header("Hunger → Chromatic Aberration")]
    [SerializeField] private float hungerThreshold = 10f;
    [SerializeField] private float chromaticAtZero = 0.45f;
    [SerializeField] private float maxCriticalChromatic = 1f;

    [Header("Common")]
    [SerializeField] private float lerpSpeed = 1.5f;

    // ───────── Volume Effects ─────────
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    // ───────── Lens Dirt Overlay ─────────
    private Material[] lensDirtMats;
    private float targetOverlayAlpha;

    // ───────── Vignette Pulse ─────────
    private bool vignettePulsing;
    private float targetVignettePulse;
    private float currentPulseSpeed;
    private float vignettePulseTimer;

    // ───────── Hunger ─────────
    private float targetChromaticIntensity;

    // ─────────────────────────────────────────────

    void Awake()
    {
        if (!Application.isPlaying) return;

        // Instanciar profile para runtime
        globalVolume.profile = Instantiate(globalVolume.profile);

        if (!globalVolume.profile.TryGet(out vignette))
            Debug.LogError("Vignette não encontrado no Volume");

        if (!globalVolume.profile.TryGet(out chromaticAberration))
            Debug.LogError("Chromatic Aberration não encontrado no Volume");

        chromaticAberration.intensity.value = 0f;

        // Criar instâncias dos materiais dos quads
        if (lensDirtRenderers != null && lensDirtRenderers.Length > 0)
        {
            lensDirtMats = new Material[lensDirtRenderers.Length];

            for (int i = 0; i < lensDirtRenderers.Length; i++)
            {
                if (lensDirtRenderers[i] == null) continue;

                lensDirtMats[i] = lensDirtRenderers[i].material;
                SetOverlayAlpha(lensDirtMats[i], 0f);
            }
        }
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

        // estado inicial
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
        UpdateLensDirt();
        UpdateVignette();
        UpdateChromatic();
    }

    // ───────── Lens Dirt Overlay ─────────

    void UpdateLensDirt()
    {
        if (lensDirtMats == null) return;

        foreach (var mat in lensDirtMats)
        {
            if (mat == null) continue;

            float current = mat.color.a;
            float next = Mathf.Lerp(
                current,
                targetOverlayAlpha,
                Time.deltaTime * lerpSpeed
            );

            SetOverlayAlpha(mat, next);
        }
    }

    void SetOverlayAlpha(Material mat, float a)
    {
        Color c = mat.color;
        c.a = a;
        mat.color = c;
    }

    void ResetOverlay()
    {
        if (lensDirtMats == null) return;

        foreach (var mat in lensDirtMats)
            SetOverlayAlpha(mat, 0f);
    }

    // ───────── Vignette ─────────

    void UpdateVignette()
    {
        if (vignette == null) return;

        if (vignettePulsing)
        {
            vignettePulseTimer += Time.deltaTime * currentPulseSpeed;

            float wave = Mathf.PingPong(vignettePulseTimer, 1f);
            vignette.intensity.value = wave * targetVignettePulse;
        }
        else
        {
            vignettePulseTimer = 0f;

            vignette.intensity.value = Mathf.Lerp(
                vignette.intensity.value,
                0f,
                Time.deltaTime * lerpSpeed
            );
        }
    }

    // ───────── Chromatic ─────────

    void UpdateChromatic()
    {
        if (chromaticAberration == null) return;

        chromaticAberration.intensity.value = Mathf.Lerp(
            chromaticAberration.intensity.value,
            targetChromaticIntensity,
            Time.deltaTime * lerpSpeed
        );
    }

    // ───────── Hygiene ─────────

    void OnHygieneChanged(float current, float max)
    {
        var manager = NeedsManager.Instance;

        // Acima do threshold → sem overlay
        if (current > hygieneThreshold)
        {
            targetOverlayAlpha = 0f;
            return;
        }

        // Entre threshold e 0 → começa a aparecer
        if (current > 0f)
        {
            float t = Mathf.InverseLerp(hygieneThreshold, 0f, current);
            targetOverlayAlpha = Mathf.Lerp(0f, maxOverlayAlpha, t);
            return;
        }

        // A 0 → cresce com o tempo (IGUAL à fome)
        float zero01 = Mathf.Clamp01(
            manager.HygieneZeroHours / manager.HoursUntilGameOver
        );

        targetOverlayAlpha = Mathf.Lerp(
            maxOverlayAlpha,
            maxCriticalOverlayAlpha,
            zero01
        );
    }



    // ───────── Energy ─────────

    void OnEnergyChanged(float current, float max)
    {
        var manager = NeedsManager.Instance;

        if (current > 0f)
        {
            vignettePulsing = false;
            targetVignettePulse = 0f;
            currentPulseSpeed = 0f;
            return;
        }

        vignettePulsing = true;

        float zero01 = Mathf.Clamp01(
            manager.EnergyZeroHours / manager.HoursUntilGameOver
        );

        targetVignettePulse = Mathf.Lerp(
            pulseAtZero,
            maxPulseIntensity,
            zero01
        );

        float pulsePeriod = Mathf.Lerp(
            pulsePeriodAtZero,
            pulsePeriodAtCritical,
            zero01
        );

        currentPulseSpeed = 1f / pulsePeriod;
    }

    // ───────── Hunger ─────────

    void OnHungerChanged(float current, float max)
    {
        var manager = NeedsManager.Instance;

        if (current > hungerThreshold)
        {
            targetChromaticIntensity = 0f;
            chromaticAberration.intensity.value = 0f;
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
