using UnityEngine;

[CreateAssetMenu(menuName = "Game/Task")]

public class TasksSO : ScriptableObject
{
    [Header("Settings")]
    public string taskName;
    public int minutesCost;
    public bool repeatable;
    public bool animalTask;

    [Header("Rewards")]
    public float energyReward;
    public float hungerReward;
    public float hygieneReward;
    public float socialReward;
    
    [Header("Costs")]
    public float energyCost;
    public float hungerCost;
    public float hygieneCost;
    public float socialCost;
}
