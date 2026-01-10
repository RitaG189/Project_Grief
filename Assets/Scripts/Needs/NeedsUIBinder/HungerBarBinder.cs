using UnityEngine;

public class HungerBarBinder : MonoBehaviour
{
    [SerializeField] private NeedsManager needsManager;
    [SerializeField] private NeedsBarUI bar;

    void OnEnable()
    {
        needsManager.OnHungerChanged += bar.UpdateBar;
    }

    void OnDisable()
    {
        needsManager.OnHungerChanged -= bar.UpdateBar;
    }
}
