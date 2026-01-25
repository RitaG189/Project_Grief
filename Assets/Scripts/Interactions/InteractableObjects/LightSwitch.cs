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
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        outline = GetComponent<Outline>();

        light?.SetActive(lightValue);
    }

    public void ToggleVisibility(bool value)
    {
        interactionText.alpha = 1;    
        
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }

        outline.enabled = value;
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
