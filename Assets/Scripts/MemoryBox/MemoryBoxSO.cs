using UnityEngine;

[CreateAssetMenu(menuName = "Game/Memory Box")]
public class MemoryBoxSO : ScriptableObject
{
    public int level;
    public MemoryItemSO[] objectsNeeded;
}
