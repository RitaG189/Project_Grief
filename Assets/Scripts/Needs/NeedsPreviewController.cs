using UnityEngine;

public class NeedsPreviewController : MonoBehaviour
{
    public static NeedsPreviewController Instance { get; private set; }

    [SerializeField] private NeedsBarUI energyBar;
    [SerializeField] private NeedsBarUI hungerBar;
    [SerializeField] private NeedsBarUI hygieneBar;
    [SerializeField] private NeedsBarUI socialBar;
    [SerializeField] private NeedsBarUI entertainmentBar;

    void Awake()
    {
        Instance = this;
    }

    public void ShowTaskPreview(TasksSO task)
    {
        var needs = NeedsManager.Instance.Needs;

        energyBar.ShowPreview(
            task.energyReward - task.energyCost,
            needs.MaxEnergy
        );

        hungerBar.ShowPreview(
            task.hungerReward - task.hungerCost,
            needs.MaxHunger
        );

        hygieneBar.ShowPreview(
            task.hygieneReward - task.hygieneCost,
            needs.MaxHygiene
        );

        socialBar.ShowPreview(
            task.socialReward - task.socialCost,
            needs.MaxSocial
        );

        entertainmentBar.ShowPreview(
            task.entertainmentReward - task.entertainmentCost,
            needs.MaxEntertainment
        );
    }

    public void ClearPreview()
    {
        energyBar.ClearPreview();
        hungerBar.ClearPreview();
        hygieneBar.ClearPreview();
        socialBar.ClearPreview();
        entertainmentBar.ClearPreview();
    }
}