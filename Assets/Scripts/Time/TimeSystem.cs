using UnityEngine;
using System;

public class TimeSystem : MonoBehaviour
{
    public int Hour { get; private set; } = 8;   // começa às 8:00
    public int Minute { get; private set; } = 0;
    public int Day { get; private set; } = 1;

    public static TimeSystem Instance { get; private set; }
    public event Action<int, int> OnTimeChanged;
    public event Action<int> OnDayChanged;

    [SerializeField] private float realSecondsPerTick = 1f;
    [SerializeField] private int minutesPerTick = 10;

    private float timer;

    void Awake()
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
        OnTimeChanged?.Invoke(Hour, Minute);
        OnDayChanged?.Invoke(Day);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realSecondsPerTick)
        {
            timer -= realSecondsPerTick;
            AdvanceMinutes(minutesPerTick);
        }
    }

    public void AdvanceMinutes(int minutes)
    {
        Minute += minutes;

        while (Minute >= 60)
        {
            Minute -= 60;
            Hour++;
        }

        if (Hour >= 24)
        {
            Hour = 0;
            Day++;
            OnDayChanged?.Invoke(Day);
        }

        OnTimeChanged?.Invoke(Hour, Minute);
    }

    public void SkipHours(int hours)
    {
        Hour += hours;

        while (Hour >= 24)
        {
            Hour -= 24;
            Day++;
            OnDayChanged?.Invoke(Day);
        }

        OnTimeChanged?.Invoke(Hour, Minute);
    }
}
