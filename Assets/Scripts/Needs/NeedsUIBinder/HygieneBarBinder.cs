using UnityEngine;

public class HygieneBarBinder : MonoBehaviour
{
    [SerializeField] private NeedsManager needsManager;
    [SerializeField] private NeedsBarUI bar;

    void OnEnable()
    {
        needsManager.OnHygieneChanged += bar.UpdateBar;
    }

    void OnDisable()
    {
        needsManager.OnHygieneChanged -= bar.UpdateBar;
    }
}
