using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MemoryBoxUI : MonoBehaviour
{
    [SerializeField] Transform contentParent;
    [SerializeField] MemoryTaskUI taskPrefab;
    [SerializeField] MemoryBox box;
    List<MemoryBoxEntry> entries = new();
    List<MemoryTaskUI> spawnedTasks = new();


    void Start()
    {
        if(box.GetCurrentBox().level == 1)
            SetBox();        
    }

    void OnEnable()
    {
        LevelsManager.OnLevelChanged += UpdateBox;
    }

    void OnDisable()
    {
        LevelsManager.OnLevelChanged -= UpdateBox;
    }

    private void UpdateBox(int level)
    {        
        if(box.GetCurrentBox().level == level)
            SetBox();
    }

    public void SetBox()
    {
        entries = box.GetRequiredItems();
        BuildUI();
    }

    void BuildUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        spawnedTasks.Clear();

        foreach (var entry in entries)
        {
            MemoryTaskUI ui = Instantiate(taskPrefab, contentParent);
            ui.Setup(entry);
            spawnedTasks.Add(ui);
        }
    }

    public void Refresh()
    {
        foreach (var task in spawnedTasks)
            task.Refresh();
    }
}
