using UnityEngine;

public class NeedsUIController : MonoBehaviour
{
    public static NeedsUIController Instance { get; private set; }

    [Header("Bars")]
    [SerializeField] private NeedsBarUI energyBar;
    [SerializeField] private NeedsBarUI hungerBar;
    [SerializeField] private NeedsBarUI hygieneBar;
    [SerializeField] private NeedsBarUI socialBar;
    [SerializeField] private NeedsBarUI emotionalEnergyBar;

    void Awake()
    {
        Instance = this;
    }

    // Chamado quando fazes hover numa task
    public void ShowTaskPreview(TasksSO task)
    {
        energyBar.ShowChange(task.energyReward);
        hungerBar.ShowChange(task.hungerReward);
        hygieneBar.ShowChange(task.hygieneReward);
        socialBar.ShowChange(task.socialReward);
        emotionalEnergyBar.ShowEmotionalEnergyChange(task.entertainmentReward, task.entertainmentCost);
    }

    // Chamado quando sais do hover / cancelas
    public void ClearPreview()
    {
        energyBar.ClearIcons();
        hungerBar.ClearIcons();
        hygieneBar.ClearIcons();
        socialBar.ClearIcons();
        emotionalEnergyBar.ClearIcons();
    }
}