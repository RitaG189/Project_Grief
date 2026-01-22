using UnityEngine;

public class NeedsSystem
{
    private NeedsConfig config;

    public float Energy { get; private set; }
    public float Hunger { get; private set; }
    public float Social { get; private set; }
    public float Hygiene { get; private set; }
    public float Entertainment { get; private set; }

    public float MaxEnergy => config.maxEnergy;
    public float MaxHunger => config.maxHunger;
    public float MaxSocial => config.maxSocial;
    public float MaxHygiene => config.maxHygiene;
    public float MaxEntertainment => config.maxEntertainment;

    public void Init(NeedsConfig config)
    {
        this.config = config;

        Energy = config.maxEnergy;
        Hunger = config.maxHunger;
        Social = config.maxSocial;
        Hygiene = config.maxHygiene;
        Entertainment = config.maxEntertainment;
    }

    public void RestoreEnergyToMax()
    {
        Energy = MaxEnergy;
    }
    
    public void RestoreHungerToMax()
    {
        Hunger = MaxHunger;
    }

    public void RestoreHygieneToMax()
    {
        Hygiene = MaxHygiene;
    }

    public void RestoreSocialToMax()
    {
        Social = MaxSocial;
    }

    public void RestoreEntertainmentToMax()
    {
        Entertainment = MaxEntertainment;
    }

    public void AddEnergy(float amount)
    {
        Energy = Mathf.Min(Energy + amount, MaxEnergy);
    }

    public void AddHunger(float amount)
    {
        Hunger = Mathf.Min(Hunger + amount, MaxHunger);
    }

    public void AddSocial(float amount)
    {
        Social = Mathf.Min(Social + amount, MaxSocial);
    }

    public void AddHygiene(float amount)
    {
        Hygiene = Mathf.Min(Hygiene + amount, MaxHygiene);
    }

    public void AddEntertainment(float amount)
    {
        Entertainment = Mathf.Min(Entertainment + amount, MaxEntertainment);
    }

    public void DecreaseEnergy(float amount)
    {
        Energy = Mathf.Max(Energy - amount, 0f);
    }

    public void DecreaseHunger(float amount)
    {
        Hunger = Mathf.Max(Hunger - amount, 0f);
    }

    public void DecreaseSocial(float amount)
    {
        Social = Mathf.Max(Social - amount, 0f);
    }

    public void DecreaseHygiene(float amount)
    {
        Hygiene = Mathf.Max(Hygiene - amount, 0f);
    }

    public void DecreaseEntertainment(float amount)
    {
        Entertainment = Mathf.Max(Entertainment - amount, 0f);
    }
}
