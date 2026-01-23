using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.Instance.CurrentState == GameState.Gameplay)
                Pause();
            else if (GameStateManager.Instance.CurrentState == GameState.Paused)
                Resume();
        }
    }

    void Pause()
    {
        GameStateManager.Instance.SetState(GameState.Paused);
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        pauseUI.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}