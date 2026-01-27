using System;
using UnityEngine;
using UnityEngine.Video;

public class MemoryCutsceneManager : MonoBehaviour
{
    public static MemoryCutsceneManager Instance {get; private set;}

    [SerializeField] private VideoPlayer videoPlayer;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlayCustcene(VideoClip clip)
    {
        videoPlayer.gameObject.SetActive(true);
        
        if (clip == null) return;

        GameStateManager.Instance.SetState(GameState.Cutscene);

        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        videoPlayer.gameObject.SetActive(false);
    }
}
