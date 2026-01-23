using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] FirstPersonLook lookScript;
    public static GameStateManager Instance;
    public GameState CurrentState {get; private set;}
    
    private void Awake() {
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
        }
    }
}

public enum GameState
{
    Gameplay,
    MainMenu,
    Paused
}
