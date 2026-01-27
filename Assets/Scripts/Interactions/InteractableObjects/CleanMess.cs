using System.Collections;
using UnityEngine;

public class CleanMess: Task
{
    [SerializeField] GameObject gameObjectCleaned;
    [SerializeField] GameObject gameObjectMessy;

    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        StartCoroutine(WaitAndDo());
    }

    IEnumerator WaitAndDo()
    {
        yield return new WaitForSeconds(1.5f);

        if(gameObjectCleaned != null)
            gameObjectCleaned.SetActive(true);

        gameObjectMessy.SetActive(false);
        Destroy(this);
    }

}
