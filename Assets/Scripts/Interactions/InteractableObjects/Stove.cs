using TMPro;
using UnityEngine;

public class Stove : Task
{
    [SerializeField] Sink sinkScript;

    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        sinkScript.SpawnDishes();
        sinkScript.EnableTask(true);
    }
}
