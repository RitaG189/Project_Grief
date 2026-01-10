using UnityEngine;

public class SocialBarBinder : MonoBehaviour
{
    [SerializeField] private NeedsManager needsManager;
    [SerializeField] private NeedsBarUI bar;

    void OnEnable()
    {
        needsManager.OnSocialChanged += bar.UpdateBar;
    }

    void OnDisable()
    {
        needsManager.OnSocialChanged -= bar.UpdateBar;
    }
}
