using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MemoryBox : MonoBehaviour, IInteractable
{
    private TMP_Text interactionText;
    [SerializeField] Transform objectPos;
    [SerializeField] int xp;
    [SerializeField] string interactionName;
    [SerializeField] MemoryBoxSO boxSO;
    [SerializeField]
    private List<MemoryBoxEntry> requiredItems = new();
    private bool canInteract = false;
    private bool boxCompleted = false;
    private bool boxClosed = false;


    [SerializeField]
    private List<MemoryItemSO> missingItems = new List<MemoryItemSO>();

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();

        requiredItems.Clear();

        foreach (var item in boxSO.objectsNeeded)
        {
            requiredItems.Add(new MemoryBoxEntry
            {
                item = item,
                done = false
            });
        }
    }
    
    public void Interact()
    {
        if(!boxCompleted)
        {
            if (!PlayerHandManager.Instance.ItemOnHand)
                return;

            MemoryBoxItem memoryItem =
                PlayerHandManager.Instance.ItemOnHand.GetComponent<MemoryBoxItem>();

            if (memoryItem == null)
                return;

            if (memoryItem.itemData.level != boxSO.level)
                return;

            if (!IsItemAccepted(memoryItem.itemData))
                return;

            PlaceItem(memoryItem);
        }
        else
        {
            CloseBox();
        }

    }


    public void ToggleVisibility(bool value)
    {
        if (interactionText == null)
            return;

        if(boxClosed)
            value = false;
        
        canInteract = false;

        if(boxCompleted)
        {
            interactionName = "Close box";
            canInteract = true;
        }    

        

        if (PlayerHandManager.Instance.ItemOnHand)
        {
            MemoryBoxItem memoryItem =
                PlayerHandManager.Instance.ItemOnHand.GetComponent<MemoryBoxItem>();

            if (memoryItem != null &&
                memoryItem.itemData.level == boxSO.level &&
                IsItemAccepted(memoryItem.itemData))
            {
                canInteract = true;
            }
        }

        interactionText.enabled = value;
        interactionText.text = interactionName;
        interactionText.alpha = canInteract ? 1f : 0.2f;
    }


    bool IsItemAccepted(MemoryItemSO item)
    {
        foreach (var entry in requiredItems)
        {
            if (entry.item == item && !entry.done)
                return true;
        }
        return false;
    }

    void MarkItemDone(MemoryItemSO item)
    {
        foreach (var entry in requiredItems)
        {
            if (entry.item == item)
            {
                entry.done = true;
                return;
            }
        }
    }

    void PlaceItem(MemoryBoxItem item)
    {
        ToggleVisibility(false);

        //item.SetPlaced(); // desativa colliders, layer, etc

        item.transform.SetParent(objectPos);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.gameObject.layer = default;

        MarkItemDone(item.itemData);

        PlayerHandManager.Instance.RemoveItemOnHand();
        LevelsManager.Instance.AddXP(xp);

        //UpdateUI();
        CheckIfCompleted();
    }

    void CheckIfCompleted()
    {
        foreach (var entry in requiredItems)
        {
            if (!entry.done)
                return;
        }

        boxCompleted = true;
        Debug.Log("Memory Box completa!");
    }

    private void CloseBox()
    {
        boxClosed = true;
        ToggleVisibility(false);
        Debug.Log("Caixa fechada");
    }

}

[System.Serializable]
public class MemoryBoxEntry
{
    public MemoryItemSO item;
    public bool done;
}