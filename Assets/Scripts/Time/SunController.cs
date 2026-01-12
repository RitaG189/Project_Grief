using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private Light sunLight;

    [SerializeField] private float sunriseAngle = -90f;
    [SerializeField] private float sunsetAngle = 270f;
    [SerializeField] float sunriseTime = 6.5f; // 06:30
    [SerializeField] float sunsetTime = 18.5f; // 18:30

    void Update()
    {
        float time01 = timeSystem.GetContinuousTime01();

        float angle = Mathf.Lerp(
            sunriseAngle,
            sunsetAngle,
            time01
        );

        sunLight.transform.rotation =
            Quaternion.Euler(angle, 170f, 0f);


        float currentTime = timeSystem.Hour + timeSystem.Minute / 60f;

        if (currentTime < sunriseTime || currentTime > sunsetTime)
        {
            sunLight.intensity = 0f;
        }
        else
        {
            float t = Mathf.InverseLerp(
                sunriseTime,
                sunsetTime,
                currentTime
            );

            sunLight.intensity = Mathf.Sin(t * Mathf.PI);
        }
    }
}
