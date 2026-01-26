using UnityEngine;

public class PlayerHandManager : MonoBehaviour
{
    [SerializeField] Transform playerHand;
    public static PlayerHandManager Instance {get; private set;}
    public bool IsHoldingItem {get; private set;} = false;
    public GameObject ItemOnHand {get; private set;}

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetItemOnHand(GameObject obj)
    {
        if(IsHoldingItem)
            return; 
            
        IsHoldingItem = true;
        ItemOnHand = obj;

        ItemOnHand.layer = LayerMask.NameToLayer("HeldItem");
        ItemOnHand.transform.SetParent(playerHand);
        ItemOnHand.transform.localPosition = Vector3.zero;
        ItemOnHand.transform.localRotation = Quaternion.identity;
    }

    public void RemoveItemOnHand()
    {
        ItemOnHand.layer = LayerMask.NameToLayer("Default");
        IsHoldingItem = false;
        ItemOnHand = null;
    }
}
