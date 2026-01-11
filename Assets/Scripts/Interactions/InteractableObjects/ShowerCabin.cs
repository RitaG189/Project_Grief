using UnityEngine;

public class ShowerCabin : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    public void Interact()
    {
        Debug.Log("Tomer banho");
        NeedsManager.Instance.TakeBath();
    }

    public void ToggleVisibility(bool value)
    {
        canvas.SetActive(value);
    }
}
