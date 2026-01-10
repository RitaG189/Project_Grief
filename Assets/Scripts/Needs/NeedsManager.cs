using UnityEngine;
using System;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] private NeedsConfig needsConfig;
    public static NeedsManager Instance { get; private set; }
    public NeedsSystem Needs { get; private set; }

    public event Action<float, float> OnEnergyChanged;
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnSocialChanged;
    public event Action<float, float> OnHygieneChanged;

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
        int hoursToSleep = GetHoursToFullEnergy();

        TimeSystem.Instance.SkipHours(hoursToSleep);

        Needs.RestoreEnergyToMax();
        Needs.AddHunger(-hoursToSleep);
        Needs.AddHygiene(-hoursToSleep);
        Needs.AddSocial(-2);
        NotifyAll();
    }

    public bool CanPerformTask(TasksSO task)
    {
        return Needs.Energy >= task.energyCost;
    }

    public void ApplyTaskCost(TasksSO task)
    {
        Needs.AddEnergy(-task.energyCost);
        Needs.AddHunger(-task.hungerCost);
        Needs.AddHygiene(-task.hygieneCost);
        Needs.AddSocial(-task.socialCost);
        NotifyAll();
    }

}
