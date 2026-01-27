using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MemoryBox : MonoBehaviour, IInteractable
{
    private TMP_Text interactionText;
    [SerializeField] Transform objectPos;
    [SerializeField] int xp;
    [SerializeField] string interactionName;
    [SerializeField] MemoryBoxSO boxSO;
    [SerializeField] VideoClip maleDogCompletionClip;
    [SerializeField] VideoClip maleCatCompletionClip;
    [SerializeField] VideoClip femaleDogCompletionClip;
    [SerializeField] VideoClip femaleCatCompletionClip;
    [SerializeField] VideoManager videoManager;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject boxOpenedGO;
    [SerializeField] GameObject boxClosedGO;


    [SerializeField] MemoryBoxUI boxUI;
    
    [SerializeField]
    private List<MemoryBoxEntry> requiredItems = new();
    
    private bool canInteract = false;
    private bool boxCompleted = false;
    private bool boxClosed = false;
    //private Outline outline;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        
        //outline = GetComponent<Outline>();

        // outline.enabled = true;
        //outline.OutlineWidth = 0f;

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
    
    void Start()
    {
        videoManager.OnVideoFinished.AddListener(HideVideoPlayer);
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

        //outline.OutlineWidth = value ? 3f : 0f;
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

        item.transform.SetParent(objectPos);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        MarkItemDone(item.itemData);

        PlayerHandManager.Instance.RemoveItemOnHand();

        boxUI.Refresh();
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

        //MemoryBoxUI.Instance.OnBoxCompleted(this);
        Debug.Log("Memory Box completa!");
    }

    private void CloseBox()
    {
        boxClosed = true;
        ToggleVisibility(false);

        LevelsManager.Instance.LevelUp();
        
        // pausar o jogo
        if(GameChoices.Instance != null)
        {
            if (GameChoices.Instance.PetSpecies == "Dog")
            {
                videoPlayer.clip = 
                    GameChoices.Instance.PetGenre == "Male"
                    ? maleDogCompletionClip
                    : femaleDogCompletionClip;
            }
            else if (GameChoices.Instance.PetSpecies == "Cat")
            {
                videoPlayer.clip =
                    GameChoices.Instance.PetGenre == "Male"
                    ? maleCatCompletionClip
                    : femaleCatCompletionClip;
            }     

            videoManager.gameObject.SetActive(true);
            videoManager.VideoPlayerPlay();  
        }

        boxClosedGO.SetActive(true);
        boxOpenedGO.SetActive(false);
        Destroy(objectPos.gameObject);
        Debug.Log("Caixa fechada");
    }

    void HideVideoPlayer()
    {
        videoManager.gameObject.SetActive(false);
    }

    public List<MemoryBoxEntry> GetRequiredItems()
    {
        return requiredItems;
    }

    public MemoryBoxSO GetCurrentBox()
    {
        return boxSO;
    }
}

[System.Serializable]
public class MemoryBoxEntry
{
    public MemoryItemSO item;
    public bool done;
}