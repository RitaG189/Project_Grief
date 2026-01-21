using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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

    public void PlayThunder(AudioClip clip, float volume = 1f)
    {
        thunderSource.PlayOneShot(clip, volume);
    }

    public void SetInside(bool inside)
    {
        mixer.SetFloat("RainVolume", inside ? -15f : 0f);
        mixer.SetFloat("RainLowpass", inside ? 800f : 22000f);
    }
}
