using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] List<GameObject> lights;
    [SerializeField] string interactionName;

    private TMP_Text interactionText;
    private bool lightValue = false;
    private Outline outline;

    void Awake()
    {
        if (!Application.isPlaying) return;

        interactionText = GameObject.FindGameObjectWithTag("InteractionText")
                                   .GetComponent<TMP_Text>();

        outline = GetComponent<Outline>();
        outline.enabled = true;
        outline.OutlineWidth = 0f;

        SetLights(lightValue);
    }

    void SetLights(bool value)
    {
        if (lights == null) return;

        foreach (GameObject light in lights)
        {
            if (light != null)
                light.SetActive(value);
        }
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
        SetLights(lightValue);

        interactionText.text = lightValue ? "Turn Off" : "Turn On";
    }
}