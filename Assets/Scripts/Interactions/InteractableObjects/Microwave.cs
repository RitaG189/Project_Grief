using UnityEngine;

public class Microwave : Task
{
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        print("eat something");
    }
}
