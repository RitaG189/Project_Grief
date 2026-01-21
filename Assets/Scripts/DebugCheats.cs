using UnityEngine;

public class DebugCheats : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            LevelsManager.Instance.AddXP(100);

        if (Input.GetKeyDown(KeyCode.F6))
            TimeSystem.Instance.SkipHours(1);
    }
}
