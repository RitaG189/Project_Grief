using UnityEngine;

public class DayUIBinder : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private DayUI dayUI;

    void OnEnable()
    {
        timeSystem.OnDayChanged += dayUI.UpdateDay;
    }

    void OnDisable()
    {
        timeSystem.OnDayChanged -= dayUI.UpdateDay;
    }
}
