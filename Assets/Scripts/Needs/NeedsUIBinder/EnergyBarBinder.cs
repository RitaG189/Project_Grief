using UnityEngine;

public class EnergyBarBinder : MonoBehaviour
{
    [SerializeField] private NeedsManager needsManager;
    [SerializeField] private NeedsBarUI bar;

    void OnEnable()
    {
        needsManager.OnEnergyChanged += bar.UpdateBar;
    }

    void OnDisable()
    {
        needsManager.OnEnergyChanged -= bar.UpdateBar;
    }
}
