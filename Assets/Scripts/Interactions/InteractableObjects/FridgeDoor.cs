using TMPro;
using UnityEngine;

public class FridgeDoor : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;
    [SerializeField] PlayerInteractionController playerInteractionController;
    [SerializeField] Transform lookAtPos;
    private Animator animator;
    private bool doorValue = false;
    //private Outline outline;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        animator = GetComponent<Animator>();

        //outline = GetComponent<Outline>();

        //.enabled = true;
        //outline.OutlineWidth = 0f;
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
        //outline.OutlineWidth = value ? 3f : 0f;
    }

    public void Interact()
    {
        ToggleVisibility(false);
        doorValue = !doorValue;

        if(doorValue == true)
            text.text = "Close";
        else   
            text.text = "Open";

        animator.SetBool("IsOpen", doorValue);
    }    
}
