using UnityEngine;

public class LevelToGriefStageBinder : MonoBehaviour
{
    [SerializeField] private GriefStageManager griefStageManager;

    private void Start()
    {
        if (LevelsManager.Instance != null)
        {
            LevelsManager.OnLevelChanged += HandleLevelChanged;
            HandleLevelChanged(LevelsManager.Instance.level);
        }
        else
        {
            Debug.LogError("LevelsManager.Instance é null!");
        }
    }

    private void OnDisable()
    {
        if (LevelsManager.Instance != null)
            LevelsManager.OnLevelChanged -= HandleLevelChanged;
    }

    void HandleLevelChanged(int level)
    {
        GriefStage stage = level switch
        {
            1 => GriefStage.Denial,
            2 => GriefStage.Bargaining,
            3 => GriefStage.Anger,
            4 => GriefStage.Depression,
            _ => GriefStage.Acceptance
        };

        griefStageManager.SetStage(stage);
    }
}

public enum GriefStage
{
    Denial,
    Bargaining,
    Anger,
    Depression,
    Acceptance
}
