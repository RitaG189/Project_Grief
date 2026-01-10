using UnityEngine;
using TMPro;

public class DayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;

    public void UpdateDay(int day)
    {
        dayText.text = $"Day: {day}";
    }
}
