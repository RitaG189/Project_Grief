using TMPro;
using UnityEngine;

public abstract class Task : MonoBehaviour, IInteractable
{
    [SerializeField] private TasksSO taskSO;
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;

    void Awake()
    {
        text.text = taskSO.taskName; 
    }

    public void Interact()
    {
        if (!TaskManager.Instance.TryExecuteSimpleTask(taskSO))
            return;

        // Lógica específica
        ExecuteTask();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }

    protected abstract void ExecuteTask();
}
