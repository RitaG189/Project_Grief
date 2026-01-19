using System;
using TMPro;
using UnityEngine;

public class Couch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;
    [SerializeField] string name;
    [SerializeField] FirstPersonMovement movement;
    [SerializeField] Transform sitPoint;
    [SerializeField] Transform lookAtPoint;

    void Awake()
    {
        text.text = name;
    }

    void Update()
    {
        if(movement.IsSitted == false)
        {
            canvas.SetActive(true);  
        }
    }

    public void Interact()
    {
        print(movement.IsSitted);
        if(movement.IsSitted == false)
        {
            PlayerInteractionController.Instance.SitOnCouch(
                lookAtPoint
            );
            canvas.SetActive(false);  
        }
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
