using UnityEngine;

public class Walk : Task
{
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        print("walked");
    }
}
