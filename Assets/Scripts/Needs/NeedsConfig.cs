using UnityEngine;

[CreateAssetMenu(menuName = "Game/Needs Config")]

public class NeedsConfig : ScriptableObject
{
    [Header("Maximum Values")]
    public float maxEnergy;
    public float maxHunger;
    public float maxHygiene;
    public float maxSocial;

    [Header("Daily Decay")]
    public float hungerDailyDecay;
    public float hygieneDailyDecay;
    public float socialDailyDecay;
}
