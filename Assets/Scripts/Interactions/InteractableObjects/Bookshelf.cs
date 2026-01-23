using UnityEngine;

public class Bookshelf : Task
{
    protected override void ExecuteTask()
    {
        print("read book");
        ToggleVisibility(false);
    }
}
