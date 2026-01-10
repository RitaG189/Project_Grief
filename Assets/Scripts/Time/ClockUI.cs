using UnityEngine;
using TMPro;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    public void UpdateClock(int hour, int minute)
    {
        timeText.text = $"{hour:00}:{minute:00}";
    }
}
