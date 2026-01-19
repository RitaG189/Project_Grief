using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutManager : MonoBehaviour
{
    public static BlackoutManager Instance { get; private set; }

    [SerializeField] float fadeDuration = 1f;
    [SerializeField] FirstPersonMovement movementScript;
    [SerializeField] FirstPersonLook lookScript;

    private Image blackout;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        blackout = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        movementScript.DisableMovement();
        lookScript.DisableLook();

        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f);

        movementScript.EnableMovement();
        lookScript.EnableLook();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color color = blackout.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            blackout.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        blackout.color = color;
    }
}
