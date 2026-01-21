using UnityEngine;

public class TimeUIBinder : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private ClockUI clockUI;

    void OnEnable()
    {
        timeSystem.OnTimeChanged += clockUI.UpdateClock;
    }

    void OnDisable()
    {
        timeSystem.OnTimeChanged -= clockUI.UpdateClock;
    }
}
