using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class TypewriterCaption : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI captionText;

    [Header("Settings")]
    [SerializeField] private float letterDelay = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping;
    public event Action OnCaptionFinished;
    public static TypewriterCaption Instance {get; private set;}

    [Header("Timing")]
    [SerializeField] private float stayAfterTyping = 2f;

    [Header("Typing Pauses")]
    [SerializeField] private float punctuationPauseMultiplier = 6f;
    [SerializeField] private float commaPauseMultiplier = 3f;
    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 0.25f;
    [SerializeField] private float fadeOutDuration = 0.25f;

    private string animalName;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (GameChoices.Instance != null && 
            !string.IsNullOrEmpty(GameChoices.Instance.PetName))
        {
            animalName = GameChoices.Instance.PetName;
        }
        else
        {
            animalName = "[Animal]";
        }
    }

    public void ShowCaption(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        string formattedText = FormatText(text);
        typingCoroutine = StartCoroutine(TypeText(formattedText));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        captionText.text = text; // necessário para medir/fade
        captionText.maxVisibleCharacters = 0;

        // Começa invisível
        Color c = captionText.color;
        c.a = 0f;
        captionText.color = c;

        // Fade In
        yield return FadeAlpha(0f, 1f, fadeInDuration);

        // Typewriter real
        int totalChars = captionText.text.Length;

        for (int i = 0; i < totalChars; i++)
        {
            captionText.maxVisibleCharacters++;

            char currentChar = captionText.text[i];
            float delay = letterDelay;

            if (currentChar == '.' || currentChar == '!' || currentChar == '?')
                delay *= punctuationPauseMultiplier;
            else if (currentChar == ',' || currentChar == ';' || currentChar == ':')
                delay *= commaPauseMultiplier;

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;

        yield return new WaitForSeconds(stayAfterTyping);

        // Fade Out
        yield return FadeAlpha(1f, 0f, fadeOutDuration);

        captionText.text = "";
        captionText.maxVisibleCharacters = 0;

        OnCaptionFinished?.Invoke();
    }


    public void Skip(string fullText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        captionText.text = fullText;
        captionText.maxVisibleCharacters = fullText.Length;

        Color c = captionText.color;
        c.a = 1f;
        captionText.color = c;

        isTyping = false;

        typingCoroutine = StartCoroutine(SkipClearRoutine());
    }

    IEnumerator SkipClearRoutine()
    {
        yield return new WaitForSeconds(stayAfterTyping);
        yield return FadeAlpha(1f, 0f, fadeOutDuration);

        captionText.text = "";
        captionText.maxVisibleCharacters = 0;

        OnCaptionFinished?.Invoke();
    }


    IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float time = 0f;
        Color color = captionText.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            color.a = Mathf.Lerp(from, to, t);
            captionText.color = color;

            yield return null;
        }

        color.a = to;
        captionText.color = color;
    }

    string FormatText(string rawText)
    {
        return rawText.Replace("{animal}", animalName);
    }

}
