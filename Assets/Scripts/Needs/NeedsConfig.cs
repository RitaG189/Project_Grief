using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Needs Config")]

public class NeedsConfig : ScriptableObject
{
    [Header("Maximum Values")]
    public float maxEnergy;
    public float maxHunger;
    public float maxHygiene;
    public float maxSocial;
    public float maxEntertainment;

    [Header("Daily Decay")]
    public float hungerDailyDecay;
    public float hygieneDailyDecay;
    public float socialDailyDecay;
    public float entertainmentDecay;

    [Header("Start Values")]
    public float startEnergy;
    public float startHunger;
    public float startHygiene;
    public float startSocial;
    public float startEntertainment;
}
