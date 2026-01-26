using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class TVRemote : MonoBehaviour, IInteractable
{
    private Outline outline;
    protected TMP_Text interactionText;
    protected bool canInteract = false;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string taskName;
    public bool Tv {get; private set;}= false;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        
        outline = GetComponent<Outline>();

        outline.enabled = true;
        outline.OutlineWidth = 0f;
    }

    public void Interact()
    {
        ToggleVisibility(false);

        ToggleTV();
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null)
        {
            interactionText.enabled = value;

            if(Tv == false)
                interactionText.text = "Turn on tv"; 
            else 
                interactionText.text = "Turn off tv"; 
        }

        outline.OutlineWidth = value ? 3f : 0f;
    }

    private void ToggleTV()
    {
        if(Tv == true)
            Tv = false;
        else 
            Tv = true;
    }
}
