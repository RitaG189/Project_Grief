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
            // ir até ao sitio do sofá
            // qd estiver no sítio certo
            // rodar em direçao à tv
            PlayerInteractionController.Instance.SitOnCouch(
                sitPoint,
                lookAtPoint
            );
            canvas.SetActive(false);  
        }
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
        text.text = name;
    }
}
