using UnityEngine;

public class SpeciesSetup : MonoBehaviour
{
    [SerializeField] GameObject catItem;
    [SerializeField] GameObject dogItem;

    void Start()
    {
        if(GameChoices.Instance != null)
        {
            if(GameChoices.Instance.PetSpecies == "Dog")
            {
                dogItem.SetActive(true);
                catItem.SetActive(false);
            }
            else if(GameChoices.Instance.PetSpecies == "Cat")
            {
                dogItem.SetActive(false);
                catItem.SetActive(true); 
            }
        }
        else
        {
            dogItem.SetActive(true);
            catItem.SetActive(false);
        }
    }
}
