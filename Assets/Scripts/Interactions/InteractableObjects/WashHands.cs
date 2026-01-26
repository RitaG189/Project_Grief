using UnityEngine;

public class WashHands : Task
{
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        print("Wash hands");
    }
}
