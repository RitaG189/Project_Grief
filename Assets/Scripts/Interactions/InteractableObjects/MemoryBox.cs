using TMPro;
using UnityEngine;

public class MemoryBox : MonoBehaviour, IInteractable
{
    private TMP_Text interactionText;
    [SerializeField] Transform objectPos;
    [SerializeField] int xp;
    [SerializeField] string interactionName;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }
    
    public void Interact()
    {
        if(PlayerHandManager.Instance.ItemOnHand)
        {
            PlayerHandManager.Instance.ItemOnHand.transform.SetParent(objectPos);
            PlayerHandManager.Instance.ItemOnHand.transform.localPosition = Vector3.zero;
            PlayerHandManager.Instance.ItemOnHand.transform.localRotation = Quaternion.identity;

            PlayerHandManager.Instance.RemoveItemOnHand();
            LevelsManager.Instance.AddXP(xp);
        }
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }
    }
}
