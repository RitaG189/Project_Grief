using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

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

    public void TryExecuteSimpleTask(TasksSO task)
    {
        if (!NeedsManager.Instance.CanPerformTask(task))
            return;

        //BlackoutManager.Instance.FadeInWaitFadeOut();
        NeedsManager.Instance.ApplyTaskCost(task);
        TimeSystem.Instance.AdvanceMinutes(task.minutesCost);

        //OnTaskCompleted?.Invoke(task);
    }
}
