using UnityEngine;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> cars;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("Spawn Timing")]
    [SerializeField] float minSpawnTime = 1.5f;
    [SerializeField] float maxSpawnTime = 4f;

    [Header("Lane")]
    [SerializeField] float laneOffset = 0f;

    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnCar();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnCar()
    {
        if (cars == null || cars.Count == 0) return;

        GameObject prefab = cars[Random.Range(0, cars.Count)];

        Vector3 spawnPos = pointA.position +
                           pointA.right * Random.Range(-laneOffset, laneOffset);

        GameObject car = Instantiate(prefab, spawnPos, pointA.rotation);

        CarMovement movement = car.GetComponent<CarMovement>();
        movement.Init(pointB);
    }
}