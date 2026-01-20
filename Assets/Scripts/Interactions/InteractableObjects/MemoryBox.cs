using TMPro;
using UnityEngine;

public class MemoryBox : MonoBehaviour, IInteractable
{
    private TMP_Text interactionText;
    [SerializeField] Transform objectPos;
    [SerializeField] int xp;
    [SerializeField] string interactionName;
    private bool canInteract = false;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }
    
    public void Interact()
    {
        if(PlayerHandManager.Instance.ItemOnHand)
        {
            ToggleVisibility(false);

            PlayerHandManager.Instance.ItemOnHand.GetComponent<AnimalTask>().IsOnBox = true;
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
            if (!PlayerHandManager.Instance.ItemOnHand)
                canInteract = false;
            else 
                canInteract = true;

            interactionText.enabled = value;
            interactionText.text = interactionName;

            if(canInteract)
            {
                interactionText.alpha = 1;              
            }
            else if(!canInteract)
            {
                interactionText.alpha = .2f; 
            }
        }
    }
}
