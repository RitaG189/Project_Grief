using UnityEngine;

public class LevelToGriefStageBinder : MonoBehaviour
{
    [SerializeField] private GriefStageManager griefStageManager;

    private void OnEnable()
    {
        LevelsManager.Instance.OnLevelChanged += HandleLevelChanged;
    }

    private void OnDisable()
    {
        if (LevelsManager.Instance != null)
            LevelsManager.Instance.OnLevelChanged -= HandleLevelChanged;
    }

    void Start()
    {
        // aplica o estado inicial
        HandleLevelChanged(LevelsManager.Instance.level);
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
