using TMPro;
using UnityEngine;

public class Task : MonoBehaviour, IInteractable
{
    [SerializeField] private TasksSO taskSO;
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;

    public void Interact()
    {
        TaskManager.Instance.TryExecuteSimpleTask(taskSO);
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
        text.text = taskSO.taskName;
    }
}
