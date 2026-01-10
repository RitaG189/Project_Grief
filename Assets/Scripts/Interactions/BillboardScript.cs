using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(
            transform.position + Camera.main.transform.forward
        );
    }
}
