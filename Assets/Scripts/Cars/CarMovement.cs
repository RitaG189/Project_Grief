using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Transform target;
    public float speed = 5f;

    public void Init(Transform targetPoint)
    {
        target = targetPoint;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Quando chega ao ponto B, destrói
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}