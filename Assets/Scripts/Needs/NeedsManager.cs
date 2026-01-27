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
    public event Action<float, float> OnEntertainmentChanged;
    public float HygieneZeroHours => hygieneZeroHours;
    public float EnergyZeroHours => energyZeroHours;
    public float HungerZeroHours => hungerZeroHours;

    public float HoursUntilGameOver => hoursUntilGameOver;

    private int lastMinuteTick = -1;

    private float energyZeroHours;
    private float hungerZeroHours;
    private float socialZeroHours;
    private float hygieneZeroHours;
    private float entertainmentZeroHours;
    private int lastTotalMinutes = -1;
    [SerializeField] private float hoursUntilGameOver = 24f;

    private void Awake()
    {
        if (!Application.isPlaying) return;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

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
        int currentTotalMinutes = GetTotalMinutes(hour, minute, TimeSystem.Instance.Day);

        if (lastTotalMinutes < 0)
        {
            lastTotalMinutes = currentTotalMinutes;
            return;
        }

        int minutesPassed = currentTotalMinutes - lastTotalMinutes;

        if (minutesPassed <= 0)
            return;

        lastTotalMinutes = currentTotalMinutes;

        float hoursPassed = minutesPassed / 60f;

        ApplyDecay(hoursPassed);
        UpdateZeroTimers(hoursPassed);
        CheckGameOver();
        NotifyAll();
    }

    void ApplyDecay(float hoursPassed)
    {
        float decay = decayPerTick * (hoursPassed * 60f / minutesPerTick);

        Needs.DecreaseEnergy(decay);
        Needs.DecreaseHunger(decay);
        Needs.DecreaseSocial(decay);
        Needs.DecreaseHygiene(decay);
    }

    void NotifyAll()
    {
        OnEnergyChanged?.Invoke(Needs.Energy, Needs.MaxEnergy);
        OnHungerChanged?.Invoke(Needs.Hunger, Needs.MaxHunger);
        OnSocialChanged?.Invoke(Needs.Social, Needs.MaxSocial);
        OnHygieneChanged?.Invoke(Needs.Hygiene, Needs.MaxHygiene);
        OnEntertainmentChanged?.Invoke(Needs.Entertainment, Needs.MaxEntertainment);
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

    public bool CanPerformAnimalTask(TasksSO task)
    {
        return Needs.Entertainment >= task.entertainmentCost;
    }

    public bool CanEat()
    {
        return Needs.Hunger <= 90;
    }

    public void ApplyTaskCostAndRewards(TasksSO task)
    {
        Needs.AddEnergy(task.energyReward);
        Needs.AddHunger(task.hungerReward);
        Needs.AddHygiene(task.hygieneReward);
        Needs.AddSocial(task.socialReward);
        Needs.AddEntertainment(task.entertainmentReward);

        Needs.DecreaseEnergy(task.energyCost);
        Needs.DecreaseHunger(task.hungerCost);
        Needs.DecreaseHygiene(task.hygieneCost);
        Needs.DecreaseSocial(task.socialCost);
        Needs.DecreaseEntertainment(task.entertainmentCost);

        NotifyAll();
    }

    public void MaxAll()
    {
        Needs.RestoreEnergyToMax();
        Needs.RestoreEntertainmentToMax();
        Needs.RestoreHungerToMax();
        Needs.RestoreHygieneToMax();
        Needs.RestoreSocialToMax();
        NotifyAll();
    }

    public void DecreaseEnergy()
    {
        Needs.DecreaseEnergy(10);
    }

    void UpdateZeroTimers(float hoursPassed)
    {
        energyZeroHours = Needs.Energy <= 0 ? energyZeroHours + hoursPassed : 0f;
        hungerZeroHours = Needs.Hunger <= 0 ? hungerZeroHours + hoursPassed : 0f;
        socialZeroHours = Needs.Social <= 0 ? socialZeroHours + hoursPassed : 0f;
        hygieneZeroHours = Needs.Hygiene <= 0 ? hygieneZeroHours + hoursPassed : 0f;
    }

    void CheckGameOver()
    {
        if (energyZeroHours >= hoursUntilGameOver ||
            hungerZeroHours >= hoursUntilGameOver ||
            socialZeroHours >= hoursUntilGameOver ||
            hygieneZeroHours >= hoursUntilGameOver)     
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        Debug.Log("GAME OVER – Need neglected too long");

        //GameManager.Instance.GameOver();
    }

    int GetTotalMinutes(int hour, int minute, int day)
    {
        return day * 24 * 60 + hour * 60 + minute;
    }
}
