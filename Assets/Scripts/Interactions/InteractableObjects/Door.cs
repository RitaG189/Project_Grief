using System;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    [SerializeField] Transform door;
    [SerializeField] private float openAngle = 90f;

    private bool isOpen = false;

    public void Interact()
    {
        if (isOpen)
        {
            door.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            door.localRotation = Quaternion.Euler(0, openAngle, 0);
        }

        isOpen = !isOpen;
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
