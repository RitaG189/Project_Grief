using UnityEngine;

public class EntertainmentBarBinder : MonoBehaviour
{
    [SerializeField] private NeedsManager needsManager;
    [SerializeField] private NeedsBarUI bar;

    void OnEnable()
    {
        needsManager.OnEntertainmentChanged += bar.UpdateBar;
    }

    void OnDisable()
    {
        needsManager.OnEntertainmentChanged -= bar.UpdateBar;
    }
}
