using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutManager : MonoBehaviour
{
    public static BlackoutManager Instance { get; private set; }

    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float waitTime = 1f;

    private Image blackout;
    private Coroutine routine;

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

    public void FadeInWaitFadeOut()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade In (0 -> 1)
        yield return Fade(0f, 1f);

        // Espera
        yield return new WaitForSeconds(waitTime);

        // Fade Out (1 -> 0)
        yield return Fade(1f, 0f);
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
