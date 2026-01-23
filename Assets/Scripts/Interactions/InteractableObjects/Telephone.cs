using UnityEngine;

public class Telephone : Task
{
    [SerializeField] GameObject telephoneMenu;
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        ToggleTelephoneMenu(true);
    }

    private void ToggleTelephoneMenu(bool value)
    {
        telephoneMenu.SetActive(value);
        
        if(value == true)
            GameStateManager.Instance.SetState(GameState.Paused);
        else 
            GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void CloseTelephone()
    {
        ToggleTelephoneMenu(false);
    }
}
