using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shower : MonoBehaviour, IInteractable
{   [SerializeField] TasksSO task;
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;

    void Awake()
    {
        text.text = task.taskName;
    }

    public void Interact()
    {
        TaskManager.Instance.TryExecuteSimpleTask(task);
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
