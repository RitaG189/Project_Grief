using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoManager videoManager;

    [SerializeField] private string nextSceneName = "MainGame";

    [SerializeField] private VideoClip clipDogFemale;
    [SerializeField] private VideoClip clipCatFemale;
    [SerializeField] private VideoClip clipDogMale;
    [SerializeField] private VideoClip clipCatMale;

    void Start()
    {
        videoManager.OnVideoFinished.AddListener(HideVideoPlayer);

        if (GameChoices.Instance != null)
        {
            if (GameChoices.Instance.PetSpecies == "Dog")
            {
                videoPlayer.clip = 
                    GameChoices.Instance.PetGenre == "Male"
                    ? clipDogMale
                    : clipDogFemale;
            }
            else if (GameChoices.Instance.PetSpecies == "Cat")
            {
                videoPlayer.clip =
                    GameChoices.Instance.PetGenre == "Male"
                    ? clipCatMale
                    : clipCatFemale;
            }
        }

        videoPlayer.Play();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            SceneManager.LoadScene(nextSceneName);
    }

    void HideVideoPlayer()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}