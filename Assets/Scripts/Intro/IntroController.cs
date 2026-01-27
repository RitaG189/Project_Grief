using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "MainGame";
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (Input.anyKeyDown)
            SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}