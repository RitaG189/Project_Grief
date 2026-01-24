using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutManager : MonoBehaviour
{
    public static BlackoutManager Instance { get; private set; }
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] FirstPersonMovement movementScript;
    [SerializeField] FirstPersonLook lookScript;

    private Image blackout;
    private TMP_Text text;


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
        text = GetComponentInChildren<TMP_Text>();
    }

    public IEnumerator FadeIn()
    {
        movementScript.DisableMovement();
        lookScript.DisableLook();
        lookScript.LockClick();

        yield return FadeImage(0f, 1f);
        yield return FadeText(text, 0f, 1f);
    }

    public IEnumerator FadeOut()
    {
        yield return FadeText(text, 1f, 0f);
        yield return FadeImage(1f, 0f);

        movementScript.EnableMovement();
        lookScript.EnableLook();
        lookScript.UnlockClick();
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha)
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

    IEnumerator FadeText(TMP_Text txt, float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color color = txt.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            txt.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        txt.color = color;
    }

    public void SetTime()
    {
        int hour = TimeSystem.Instance.Hour;
        int minute = TimeSystem.Instance.Minute;

        text.text = $"{hour:00}:{minute:00}";
    }
}
