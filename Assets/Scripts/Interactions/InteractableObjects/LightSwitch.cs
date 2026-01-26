using TMPro;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject light;
    [SerializeField] string interactionName;
    private TMP_Text interactionText;
    private bool lightValue = false;
    private Outline outline;

    void Awake()
    {
        if (!Application.isPlaying) return;

        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        
        outline = GetComponent<Outline>();

        outline.enabled = true;
        outline.OutlineWidth = 0f;

        light?.SetActive(lightValue);
    }

    public void ToggleVisibility(bool value)
    {
        interactionText.alpha = 1;

        if (interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }

        outline.OutlineWidth = value ? 3f : 0f;
    }

    public void Interact()
    {
        ToggleVisibility(false);
        lightValue = !lightValue;

        light.SetActive(lightValue);

        if(lightValue == false)
            interactionText.text = "Turn On";
        else
            interactionText.text = "Turn Off";
          
    }
}
