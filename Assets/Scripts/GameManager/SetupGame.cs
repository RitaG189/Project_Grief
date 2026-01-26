using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetupGame : MonoBehaviour
{
    [SerializeField] GameChoices gc;
    [Header("Fields")]
    [SerializeField] TMP_InputField petName;
    [SerializeField] TMP_Dropdown petSpecies;
    [SerializeField] TMP_Dropdown petGenre;

    [Header("Colors")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color errorColor = new Color(1f, 0.6f, 0.6f);

    void Start()
    {
        // Limpar erro ao clicar / alterar
        petName.onSelect.AddListener(_ => ClearFieldError(petName.image));

        petSpecies.onValueChanged.AddListener(_ => ClearFieldError(petSpecies.image));
        petGenre.onValueChanged.AddListener(_ => ClearFieldError(petGenre.image));
    }

    public void ConfirmInfo()
    {
        if (!ValidateForm())
            return;

        // Aqui podes continuar o jogo
        Debug.Log("Form válido!");

        gc.PetName = petName.text;
        gc.PetSpecies = petSpecies.options[petSpecies.value].text;
        gc.PetGenre = petGenre.options[petGenre.value].text;

        SceneManager.LoadScene("Sims");
    }

    bool ValidateForm()
    {
        bool isValid = true;

        // Nome
        if (string.IsNullOrWhiteSpace(petName.text))
        {
            SetFieldError(petName.image);
            isValid = false;
        }
        else
        {
            petName.text = CapitalizeFirstLetter(petName.text.Trim());
            ClearFieldError(petName.image);
        }

        // Espécie
        if (petSpecies.value == 0)
        {
            SetFieldError(petSpecies.image);
            isValid = false;
        }
        else
        {
            ClearFieldError(petSpecies.image);
        }

        // Género
        if (petGenre.value == 0)
        {
            SetFieldError(petGenre.image);
            isValid = false;
        }
        else
        {
            ClearFieldError(petGenre.image);
        }

        return isValid;
    }

    void SetFieldError(Image image)
    {
        image.color = errorColor;
    }

    void ClearFieldError(Image image)
    {
        image.color = normalColor;
    }

    string CapitalizeFirstLetter(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return char.ToUpper(text[0]) + text.Substring(1);
    }
}


