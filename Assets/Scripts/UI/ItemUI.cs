using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image checkmark;

    MemoryBoxItem item;

    public void Setup(MemoryBoxItem data)
    {
        item = data;
        Refresh();
    }

    public void Refresh()
    {
        titleText.text = item.itemData.name;
        descriptionText.text = item.itemData.description;
        //checkmark.enabled = item.itemData.name;
    }
}
