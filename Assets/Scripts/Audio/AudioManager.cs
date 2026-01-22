using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioMixer mixer;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    public AudioSource rainSource;
    public AudioSource thunderSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySound(SoundSO sound)
    {
        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void PlayRain(SoundSO rainSound)
    {
        if (rainSource.isPlaying &&
            rainSource.clip == rainSound.clip) return;

        rainSource.clip = rainSound.clip;
        rainSource.volume = rainSound.volume;
        rainSource.loop = true;
        rainSource.Play();
    }

    public void StopRain()
    {
        rainSource.Stop();
    }

    public void PlayThunder(SoundSO[] thunderSounds)
    {
        if (thunderSounds == null || thunderSounds.Length == 0)
            return;

        SoundSO sound = thunderSounds[Random.Range(0, thunderSounds.Length)];
        thunderSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void SetInside(bool inside)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInside(inside));
    }

    IEnumerator FadeInside(bool inside)
    {
        float targetVol = inside ? -15f : 0f;
        float targetLPF = inside ? 800f : 22000f;

        mixer.GetFloat("RainVolume", out float startVol);
        mixer.GetFloat("RainLowpass", out float startLPF);

        float t = 0f;
        while (t < .1f)
        {
            t += Time.deltaTime;
            mixer.SetFloat("RainVolume", Mathf.Lerp(startVol, targetVol, t));
            mixer.SetFloat("RainLowpass", Mathf.Lerp(startLPF, targetLPF, t));
            yield return null;
        }
    }
}
