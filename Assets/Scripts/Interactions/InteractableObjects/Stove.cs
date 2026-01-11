using UnityEngine;

public class Stove : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    public void Interact()
    {
        Debug.Log("Comer");
        NeedsManager.Instance.Eat();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
