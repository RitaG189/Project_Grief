using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance {get; private set;}

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
