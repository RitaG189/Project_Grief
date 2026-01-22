using TMPro;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject light;
    [SerializeField] string interactionName;
    private TMP_Text interactionText;
    private bool lightValue = false;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        light.SetActive(lightValue);
    }

    public void ToggleVisibility(bool value)
    {
        interactionText.alpha = 1;    
        
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }
    }

    public void Interact()
    {
        ToggleVisibility(false);
        lightValue = !lightValue;

        light.SetActive(lightValue);

        if(lightValue == false)
            text.text = "Off";
        else
            text.text = "On";
          
    }
}
