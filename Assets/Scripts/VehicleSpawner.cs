using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> vehicles;
    [SerializeField] private Transform spawnPosCar;
    [SerializeField] private Transform spawnPosTruck;
    private int leftOrRight;
    private float currentCarSpeed;
    private float currentTruckSpeed;

    // Start is called before the first frame update
    void Start()
    {
        leftOrRight = Random.Range(0, 2);
        currentCarSpeed = 4f;
        currentTruckSpeed = 2.5f;
        StartCoroutine(SpawnVehicle());
        StartCoroutine(SpeedUp());
    }

    // Update is called once per frame
    private IEnumerator SpawnVehicle()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 6.7f));
            GameObject vehicle = vehicles[leftOrRight];
            if (leftOrRight == 0)
            {
                vehicle.GetComponent<VehicleBehaviour>().defaultSpeed = currentCarSpeed;
                Instantiate(vehicle, spawnPosCar.position, vehicle.transform.rotation).transform.parent = gameObject.transform;
            }
            else
            {
                vehicle.GetComponent<VehicleBehaviour>().defaultSpeed = currentTruckSpeed;
                Instantiate(vehicle, spawnPosTruck.position, vehicle.transform.rotation).transform.parent = gameObject.transform;
            }
        }
    }

    private IEnumerator SpeedUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            currentCarSpeed += 0.3f;
            currentTruckSpeed += 0.3f;
        }
    }
}
