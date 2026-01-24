using UnityEngine;

public class PlayerInsideDetector : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float checkDistance = 3f;
    [SerializeField] private LayerMask roofLayer;

    [Header("State")]
    public bool IsInside { get; private set; }

    void Update()
    {
        CheckInside();
    }

    void CheckInside()
    {
        bool wasInside = IsInside;

        // Raycast para cima
        IsInside = Physics.Raycast(
            transform.position,
            Vector3.up,
            checkDistance,
            roofLayer
        );

        // Só reage quando muda de estado
        if (IsInside != wasInside)
        {
            if(AudioManager.Instance != null)
                AudioManager.Instance.SetInside(IsInside);
        }
    }

#if UNITY_EDITOR
    // Debug visual no Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = IsInside ? Color.green : Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.up * checkDistance
        );
    }
#endif
}
