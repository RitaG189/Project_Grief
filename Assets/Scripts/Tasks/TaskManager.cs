using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    private PauseController pauseController;
    [SerializeField] FirstPersonLook firstLook;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        pauseController = GetComponent<PauseController>();
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

        // Fade to black
        firstLook.LockClick();
        BlackoutManager.Instance.SetTime();
        pauseController.CanPause = false;
        yield return BlackoutManager.Instance.FadeIn();

        bool captionFinished = false;

        void OnCaptionDone()
        {
            captionFinished = true;
        }
        

        TypewriterCaption.Instance.OnCaptionFinished += OnCaptionDone;

        // Mostrar caption
        CaptionByLevel.Instance.ShowCaptionForCurrentLevel(task);


        // Espera ATÉ a caption acabar
        yield return new WaitUntil(() => captionFinished);

        TypewriterCaption.Instance.OnCaptionFinished -= OnCaptionDone;

        yield return new WaitForSeconds(1f);

        NeedsManager.Instance.ApplyTaskCostAndRewards(task);
        TimeSystem.Instance.AdvanceMinutes(task.minutesCost);

        BlackoutManager.Instance.SetTime();

        yield return new WaitForSeconds(1f);

        yield return BlackoutManager.Instance.FadeOut();
        pauseController.CanPause = true;
        firstLook.UnlockClick();
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
