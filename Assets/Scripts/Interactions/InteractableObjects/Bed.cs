using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;
    [SerializeField] String taskName;
    public void Interact()
    {
        NeedsManager.Instance.Sleep();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
        text.text = taskName;
    }
}