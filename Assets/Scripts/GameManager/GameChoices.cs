using UnityEngine;

public class GameChoices : MonoBehaviour
{  
    public string Genre {get; set;} = "Male";
    public string PetName {get; set;}
    public string PetSpecies {get; set;} = "Dog";
    public string PetGenre {get; set;} = "Male";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


}
