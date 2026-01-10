using System;
using TMPro;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    public void Interact()
    {
        Debug.Log("Dormir");
        NeedsManager.Instance.Sleep();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}