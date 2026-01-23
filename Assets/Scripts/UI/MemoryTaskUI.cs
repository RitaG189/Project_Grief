using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryTaskUI : MonoBehaviour
{
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image checkmark;

    MemoryBoxEntry entry;

    public void Setup(MemoryBoxEntry entryData)
    {
        entry = entryData;
        Refresh();
    }

    public void SetupSimple(string text, bool completed = false)
    {
        descriptionText.text = text;
        checkmark.enabled = completed;
    }

    public void Refresh()
    {
        descriptionText.text = entry.item.description;
        checkmark.enabled = entry.done;
        descriptionText.alpha = entry.done ? 0.5f : 1f;
    }
}
