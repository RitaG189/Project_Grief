using System.Collections;
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

    public bool TryExecuteTask(TasksSO task)
    {
        if (!NeedsManager.Instance.CanPerformTask(task)) 
            return false;

        if(task.animalTask == false)
            StartCoroutine(ExecuteSimpleTaskRoutine(task));

        return true;
    }

    private IEnumerator ExecuteSimpleTaskRoutine(TasksSO task)
    {
        if (!NeedsManager.Instance.CanPerformTask(task))
            yield break;

        // Espera até o ecrã ficar completamente preto
        yield return BlackoutManager.Instance.FadeIn();

        // Agora já não há fugas visuais
        NeedsManager.Instance.ApplyTaskCostAndRewards(task);
        TimeSystem.Instance.AdvanceMinutes(task.minutesCost);

        // Pequena pausa opcional (sensação de passagem de tempo)
        yield return new WaitForSeconds(0.5f);

        yield return BlackoutManager.Instance.FadeOut();
    }

    public void Sleep()
    {
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        int hoursSlept = NeedsManager.Instance.hoursToSleep();

        yield return BlackoutManager.Instance.FadeIn();

        TimeSystem.Instance.SkipHours(hoursSlept);
        NeedsManager.Instance.Sleep();

        yield return new WaitForSeconds(0.5f);

        yield return BlackoutManager.Instance.FadeOut();
    }
}
