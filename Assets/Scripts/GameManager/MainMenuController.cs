using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Setup_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}