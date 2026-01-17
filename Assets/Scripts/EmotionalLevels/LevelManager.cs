using System;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public LevelSystem Levels { get; private set; }
    public static LevelsManager Instance { get; private set; }
    public event Action<int> OnXPChanged;
    public event Action<int> OnLevelChanged;
    public int level = 1;
    public int currentXP = 0;
    public int maxXP = 100;
    public int xpToNextLevel = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        OnXPChanged?.Invoke(currentXP);
        OnLevelChanged?.Invoke(level);
    }

    public void AddXP(int value)
    {
        currentXP += value;
        OnXPChanged?.Invoke(currentXP);

        if(currentXP >= maxXP)
        {
            level++;
            currentXP -= xpToNextLevel;
            OnXPChanged?.Invoke(currentXP);
            OnLevelChanged?.Invoke(level);
        }
    }
}
