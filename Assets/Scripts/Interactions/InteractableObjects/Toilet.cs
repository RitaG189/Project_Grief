using UnityEngine;

public class Toilet : Task
{
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        print("used toilet");
    }
}
