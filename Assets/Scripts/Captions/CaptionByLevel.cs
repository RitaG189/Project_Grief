using System.Collections.Generic;
using UnityEngine;

public class CaptionByLevel : MonoBehaviour
{
    [Header("Caption")]
    public static CaptionByLevel Instance {get; private set;}

    void Awake()
    {
        if (!Application.isPlaying) return;

        if(Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void ShowCaptionForCurrentLevel(TasksSO task)
    {
        int level = LevelsManager.Instance.level;

        List<string> selectedList = GetListByLevel(level, task);

        if (selectedList == null || selectedList.Count == 0)
        {
            Debug.LogWarning("No captions for level " + level);
            return;
        }

        string randomCaption = selectedList[Random.Range(0, selectedList.Count)];
        TypewriterCaption.Instance.ShowCaption(randomCaption);
    }

    List<string> GetListByLevel(int level, TasksSO task)
    {
        return level switch
        {
            1 => task.denialCaptions,
            2 => task.angerCaptions,
            3 => task.bargainingCaptions,
            4 => task.depressionCaptions,
            5 => task.acceptanceCaptions,
            _ => task.acceptanceCaptions
        };
    }
}
