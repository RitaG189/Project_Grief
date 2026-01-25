using System.Collections.Generic;
using UnityEngine;

public class CaptionByLevel : MonoBehaviour
{
    [Header("Caption")]
    public List<string> denialCaptions;
    public List<string> angerCaptions;
    public List<string> bargainingCaptions;
    public List<string> depressionCaptions;
    public List<string> acceptanceCaptions;
    public static CaptionByLevel Instance {get; private set;}

    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void ShowCaptionForCurrentLevel()
    {
        int level = LevelsManager.Instance.level;

        List<string> selectedList = GetListByLevel(level);

        if (selectedList == null || selectedList.Count == 0)
        {
            Debug.LogWarning("No captions for level " + level);
            return;
        }

        string randomCaption = selectedList[Random.Range(0, selectedList.Count)];
        TypewriterCaption.Instance.ShowCaption(randomCaption);
    }

    List<string> GetListByLevel(int level)
    {
        return level switch
        {
            1 => denialCaptions,
            2 => angerCaptions,
            3 => bargainingCaptions,
            4 => depressionCaptions,
            5 => acceptanceCaptions,
            _ => denialCaptions
        };
    }
}
