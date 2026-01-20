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

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }

    public void Interact()
    {
        doorValue = !doorValue;

        if(doorValue == true)
            text.text = "Close";
        else   
            text.text = "Open";

        animator.SetBool("IsOpen", doorValue);
    }    
}
