using UnityEngine;

public class MemoryBox : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
