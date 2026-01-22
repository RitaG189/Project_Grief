using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoryBox : MonoBehaviour, IInteractable
{
    private TMP_Text interactionText;
    [SerializeField] Transform objectPos;
    [SerializeField] int xp;
    [SerializeField] string interactionName;
    [SerializeField] MemoryBoxSO boxSO;
    private bool canInteract = false;

    [SerializeField]
    private List<MemoryItemSO> missingItems = new List<MemoryItemSO>();

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        
        missingItems.AddRange(boxSO.objectsNeeded);
    }
    
    public void Interact()
    {
        if (LevelsManager.Instance.level != boxSO.level)
            return;

        if (!PlayerHandManager.Instance.ItemOnHand)
            return;

        MemoryBoxItem memoryItem = PlayerHandManager.Instance.ItemOnHand
            .GetComponent<MemoryBoxItem>();

        if (memoryItem == null)
            return;

        // ❌ Se o objeto NÃO é aceite
        if (!IsItemAccepted(memoryItem.itemData))
        {
            Debug.Log("Objeto errado para esta caixa");
            return;
        }

        // ✅ Se é aceite
        PlaceItem(memoryItem);
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

    bool IsItemAccepted(MemoryItemSO item)
    {
        return missingItems.Contains(item);
    }

    void PlaceItem(MemoryBoxItem item)
    {
        ToggleVisibility(false);

        item.transform.SetParent(objectPos);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // Remove da lista de itens em falta
        missingItems.Remove(item.itemData);

        PlayerHandManager.Instance.RemoveItemOnHand();

        LevelsManager.Instance.AddXP(xp);

        CheckIfCompleted();
    }

    void CheckIfCompleted()
    {
        if (missingItems.Count == 0)
        {
            Debug.Log("Memory Box completa!");
        }
    }

}
