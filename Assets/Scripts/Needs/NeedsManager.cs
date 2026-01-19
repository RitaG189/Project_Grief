using UnityEngine;
using System;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] private NeedsConfig needsConfig;
    [SerializeField] private float decayPerTick = 0.1f;
    [SerializeField] private int minutesPerTick = 10;
    public static NeedsManager Instance { get; private set; }
    public NeedsSystem Needs { get; private set; }
    public event Action<float, float> OnEnergyChanged;
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnSocialChanged;
    public event Action<float, float> OnHygieneChanged;
    private int lastMinuteTick = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Needs = new NeedsSystem();
        Needs.Init(needsConfig);
    }

    void Start()
    {
        NotifyAll();  
        TimeSystem.Instance.OnTimeChanged += OnTimeChanged;
    }

    void OnDestroy()
    {
        TimeSystem.Instance.OnTimeChanged -= OnTimeChanged;
    }

    private void OnTimeChanged(int hour, int minute)
    {
        // Só executa de 10 em 10 minutos
        if (minute % minutesPerTick != 0)
            return;

        // Evita executar duas vezes no mesmo minuto
        if (minute == lastMinuteTick)
            return;

        lastMinuteTick = minute;
        
        Needs.DecreaseEnergy(decayPerTick);
        Needs.DecreaseHunger(decayPerTick);
        Needs.DecreaseSocial(decayPerTick);
        Needs.DecreaseHygiene(decayPerTick);

        NotifyAll();
    }

    void NotifyAll()
    {
        OnEnergyChanged?.Invoke(Needs.Energy, Needs.MaxEnergy);
        OnHungerChanged?.Invoke(Needs.Hunger, Needs.MaxHunger);
        OnSocialChanged?.Invoke(Needs.Social, Needs.MaxSocial);
        OnHygieneChanged?.Invoke(Needs.Hygiene, Needs.MaxHygiene);
    }

    public int GetHoursToFullEnergy()
    {
        return Mathf.CeilToInt(Needs.MaxEnergy - Needs.Energy);
    }

    public void Sleep()
    {
        int hours = hoursToSleep();
        Needs.RestoreEnergyToMax();
        Needs.DecreaseHunger(hours);
        Needs.DecreaseHygiene(hours);
        Needs.DecreaseSocial(2);
        NotifyAll();
    }

    public int hoursToSleep()
    {
        int hoursToSleep = GetHoursToFullEnergy();
        return hoursToSleep;
    }

    public bool CanPerformTask(TasksSO task)
    {
        return Needs.Energy >= task.energyCost;
    }

    public void ApplyTaskCostAndRewards(TasksSO task)
    {
        Needs.AddEnergy(task.energyReward);
        Needs.AddHunger(task.hungerReward);
        Needs.AddHygiene(task.hygieneReward);
        Needs.AddSocial(task.socialReward);

        Needs.DecreaseEnergy(task.energyCost);
        Needs.DecreaseHunger(task.hungerCost);
        Needs.DecreaseHygiene(task.hygieneCost);
        Needs.DecreaseSocial(task.socialCost);

        NotifyAll();
    }
}
