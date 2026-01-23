using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MemoryBoxUI : MonoBehaviour
{
    public static MemoryBoxUI Instance {get; private set;}
    [SerializeField] Transform contentParent;
    [SerializeField] MemoryTaskUI taskPrefab;
    [SerializeField] List<MemoryBox> boxes;
    List<MemoryBoxEntry> entries = new();
    List<MemoryTaskUI> spawnedTasks = new();

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }    

        Instance = this;
    }

    void Start()
    {
        foreach (MemoryBox box in boxes)
        {
            if (box == null)
                continue;

            if (box.GetCurrentBox() != null && box.GetCurrentBox().level == 1)
            {
                SetBox(box);
                break; // só queremos a primeira
            }
        }
    }

    public void SetBox(MemoryBox box)
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

    public void OnBoxCompleted(MemoryBox box)
    {
        // spawn da task extra
        MemoryTaskUI ui = Instantiate(taskPrefab, contentParent);
        ui.SetupSimple("Close box");

        spawnedTasks.Add(ui);
    }
}
