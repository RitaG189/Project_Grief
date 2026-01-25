using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

        if(task.category != TaskCategory.Animal)
            StartCoroutine(ExecuteSimpleTaskRoutine(task));

        return true;
    }
    
    public bool TryExecuteAnimalTask(TasksSO task)
    {
        if (!NeedsManager.Instance.CanPerformAnimalTask(task)) 
            return false;

        return true;
    }

    private IEnumerator ExecuteSimpleTaskRoutine(TasksSO task)
    {
        if (!NeedsManager.Instance.CanPerformTask(task))
            yield break;

        // Espera até o ecrã ficar completamente preto
        BlackoutManager.Instance.SetTime();
        yield return BlackoutManager.Instance.FadeIn();

        // Agora já não há fugas visuais
        NeedsManager.Instance.ApplyTaskCostAndRewards(task);
        TimeSystem.Instance.AdvanceMinutes(task.minutesCost);

        yield return new WaitForSeconds(0.5f);

        BlackoutManager.Instance.SetTime();
        
        // Pequena pausa opcional (sensação de passagem de tempo)
        yield return new WaitForSeconds(1f);

        yield return BlackoutManager.Instance.FadeOut();
    }

    public void Sleep(int? hours = null)
    {
        int hoursSlept = hours ?? NeedsManager.Instance.hoursToSleep();
        StartCoroutine(SleepRoutine(hoursSlept));
    }

    private IEnumerator SleepRoutine(int hoursSlept)
    {
        BlackoutManager.Instance.SetTime();
        yield return BlackoutManager.Instance.FadeIn();

        TimeSystem.Instance.SkipHours(hoursSlept);
        NeedsManager.Instance.Sleep();

        yield return new WaitForSeconds(0.5f);

        BlackoutManager.Instance.SetTime();

        yield return new WaitForSeconds(1f);

        yield return BlackoutManager.Instance.FadeOut();
    }
}
