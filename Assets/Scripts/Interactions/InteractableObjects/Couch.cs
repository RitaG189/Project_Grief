using System;
using TMPro;
using UnityEngine;

public class Couch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    private TMP_Text interactionText;
    [SerializeField] string name;
    [SerializeField] FirstPersonMovement movement;
    [SerializeField] Transform sitPoint;
    [SerializeField] Transform lookAtPoint;
    [SerializeField] TVRemote tvRemote;

    //private Outline outline;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        interactionText.text = name;

        //outline = GetComponent<Outline>();

        //outline.enabled = true;
        //outline.OutlineWidth = 0f;
    }

    void Update()
    {
        if(movement.IsSitted == true)
        {
            interactionText.enabled = false;
        }
    }

    public void Interact()
    {
        if(movement.IsSitted == false && !PlayerHandManager.Instance.IsHoldingItem)
        {
            PlayerInteractionController.Instance.SitOnCouch(
                lookAtPoint
            );
            ToggleVisibility(false);

            if(tvRemote.Tv == true)
            {
                GameStateManager.Instance.SetState(GameState.Fast);
            }

        }
    }

    public void ToggleVisibility(bool value)
    {
        interactionText.alpha = 1;    
        
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = name;
        }

        //outline.OutlineWidth = value ? 3f : 0f;
    }
}
