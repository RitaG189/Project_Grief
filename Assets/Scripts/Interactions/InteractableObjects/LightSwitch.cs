using TMPro;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject light;
    [SerializeField] float lightIntensity;
    private bool lightValue = false;

    void Awake()
    {
        light.SetActive(lightValue);
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
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
