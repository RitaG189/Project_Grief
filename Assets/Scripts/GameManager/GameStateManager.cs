using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] FirstPersonLook lookScript;
    public static GameStateManager Instance;
    public GameState CurrentState {get; private set;}
    
    private void Awake() {
        if (!Application.isPlaying) return;
        
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Gameplay:
                Time.timeScale = 1f;
                CursorUtils.DisableUICursor();
                CursorUtils.EnableLockedCursor();
                lookScript.EnableLook();
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                CursorUtils.EnableUICursor();
                CursorUtils.EnableUnlockedCursor();
                lookScript.DisableLook();
                break;

            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Cutscene:
                Time.timeScale = 0f;
                lookScript.DisableLook();
                break;
            case GameState.Fast:
                Time.timeScale = 10f;
                break;
        }
    }
}

public enum GameState
{
    Gameplay,
    MainMenu,
    Paused,
    Cutscene,
    Fast
}
