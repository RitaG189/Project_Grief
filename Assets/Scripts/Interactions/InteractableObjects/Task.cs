using UnityEngine;

public class Task : MonoBehaviour, IInteractable
{
    [SerializeField] private TasksSO taskSO;
    [SerializeField] GameObject canvas;

    public void Interact()
    {
        TaskManager.Instance.TryExecuteSimpleTask(taskSO);
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
