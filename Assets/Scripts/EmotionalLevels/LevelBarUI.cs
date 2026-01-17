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
        levelText.text = level.ToString();
    }

    void OnEnable()
    {
        if (LevelsManager.Instance == null) return;

        LevelsManager.Instance.OnXPChanged += UpdateBar;
        LevelsManager.Instance.OnLevelChanged += UpdateLevel;
    }

    void OnDisable()
    {
        if (LevelsManager.Instance == null) return;

        LevelsManager.Instance.OnXPChanged -= UpdateBar;
        LevelsManager.Instance.OnLevelChanged -= UpdateLevel;
    }
}
