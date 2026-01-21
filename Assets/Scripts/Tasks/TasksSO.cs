using UnityEngine;

[CreateAssetMenu(menuName = "Game/Task")]

public class TasksSO : ScriptableObject
{
    [Header("Settings")]
    public string taskName;
    public int minutesCost;
    public bool repeatable;
    public bool taskDone;
    public TaskCategory category; 

    [Header("Rewards")]
    public float energyReward;
    public float hungerReward;
    public float hygieneReward;
    public float socialReward;
    public float entertainmentReward;
    
    [Header("Costs")]
    public float energyCost;
    public float hungerCost;
    public float hygieneCost;
    public float socialCost;
    public float entertainmentCost;
}

public enum TaskCategory
{
    Energy,
    Hunger,
    Hygiene,
    Social,
    Entertainment,
    Animal
}