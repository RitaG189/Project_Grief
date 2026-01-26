using UnityEngine;

public class GameChoices : MonoBehaviour
{  
    public static GameChoices Instance {get; private set;}
    public string Genre {get; set;} = "Male";
    public string PetName {get; set;}
    public string PetSpecies {get; set;} = "Dog";
    public string PetGenre {get; set;} = "Male";

    void Awake()
    {
        if (!Application.isPlaying) return;

        if(Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


}
