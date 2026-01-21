using UnityEngine;


[CreateAssetMenu(menuName = "Game/Audio")]
public class SoundSO : ScriptableObject 
{
    public AudioClip clip;
    public float volume = 1f;
    public bool loop;    
}
