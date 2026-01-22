using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text levelText;

    public void UpdateBar(int currentXP)
    {
        slider.value = (float)currentXP / 100;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = level switch
        {
            1 => "Denial",
            2 => "Anger",
            3 => "Bargaining",
            4 => "Depression",
            _ => "Acceptance"
        };
    }

    void OnEnable()
    {
        if (LevelsManager.Instance == null) return;

        LevelsManager.OnXPChanged += UpdateBar;
        LevelsManager.OnLevelChanged += UpdateLevel;
    }

    void OnDisable()
    {
        if (LevelsManager.Instance == null) return;

        LevelsManager.OnXPChanged -= UpdateBar;
        LevelsManager.OnLevelChanged -= UpdateLevel;
    }
}
