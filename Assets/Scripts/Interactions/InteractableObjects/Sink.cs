using System.Collections;
using UnityEngine;

public class Sink : Task
{
    private void Awake() 
    {
        if (!Application.isPlaying) return;

        EnableTask(false); 
    }

    [SerializeField] GameObject dishes1;
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        StartCoroutine(WashDishes());
        print("cleaned dishes");

    }

    public void SpawnDishes()
    {
        dishes1.SetActive(true);
    }

    IEnumerator WashDishes()
    {
        EnableTask(false); 
        float timeUntilWash = 1;

        yield return new WaitForSeconds(timeUntilWash);

        dishes1.SetActive(false);
    }
}
