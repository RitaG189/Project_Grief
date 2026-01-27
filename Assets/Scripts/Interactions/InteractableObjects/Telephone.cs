using TMPro;
using UnityEngine;

public class Telephone : Task
{
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        print("used telephone");
    }
}
