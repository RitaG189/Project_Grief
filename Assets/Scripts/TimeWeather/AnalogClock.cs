using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;

    [Header("Offsets")]
    [SerializeField] private float hourOffset = 0f;
    [SerializeField] private float minuteOffset = 0f;

    void Start()
    {
        UpdateClock(
            TimeSystem.Instance.Hour,
            TimeSystem.Instance.Minute
        );

        TimeSystem.Instance.OnTimeChanged += UpdateClock;
    }

    void OnDestroy()
    {
        if (TimeSystem.Instance != null)
            TimeSystem.Instance.OnTimeChanged -= UpdateClock;
    }

    void UpdateClock(int hour, int minute)
    {
        // ⚠️ NOTA: SEM sinais negativos aqui
        float minuteAngle = minute * 6f + minuteOffset;
        float hourAngle =
            (hour % 12) * 30f + (minute / 60f) * 30f + hourOffset;

        // ⚠️ Eixo Z positivo = horário (para este mesh)
        minuteHand.localEulerAngles = new Vector3(0, 0, minuteAngle);
        hourHand.localEulerAngles = new Vector3(0, 0, hourAngle);
    }
}
