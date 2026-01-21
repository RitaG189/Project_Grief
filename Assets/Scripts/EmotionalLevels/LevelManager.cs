using System;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager Instance { get; private set; }

    public static event Action<int> OnXPChanged;
    public static event Action<int> OnLevelChanged;

    [Header("Level Data")]
    public int level = 1;
    public int currentXP = 0;
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

        if (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            level++;

            OnXPChanged?.Invoke(currentXP);
            OnLevelChanged?.Invoke(level);
        }
    }
}
