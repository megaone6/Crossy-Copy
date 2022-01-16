using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSpawner: MonoBehaviour
{
    [SerializeField] private List<GameObject> movingObjs;
    [SerializeField] private Transform spawnPosRight;
    [SerializeField] private Transform spawnPosLeft;
    [SerializeField] private bool waterOrRoad;
    private int leftOrRight;
    private float currentCarSpeed;
    private float currentTruckSpeed;
    private float currentLogSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentCarSpeed = 2f;
        currentTruckSpeed = 1.25f;
        currentLogSpeed = 3f;
        leftOrRight = Random.Range(0, 2);
        StartCoroutine(SpawnVehicle());
        if (!waterOrRoad)
            StartCoroutine(SpeedUp());
    }

    // Update is called once per frame
    private IEnumerator SpawnVehicle()
    {
        while(true)
        {
            if (!waterOrRoad)
                yield return new WaitForSeconds(Random.Range(4f, 6.7f));
            else
                yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            GameObject movingObj = movingObjs[leftOrRight];
            if (leftOrRight == 0)
            {
                if (!waterOrRoad)
                {
                    movingObj.GetComponent<MovingObjectBehaviour>().defaultSpeed = currentCarSpeed;
                    Instantiate(movingObj, spawnPosRight.position, movingObj.transform.rotation).transform.parent = gameObject.transform;
                }
                else
                {
                    movingObj.GetComponent<MovingObjectBehaviour>().defaultSpeed = currentLogSpeed;
                    Instantiate(movingObj, spawnPosRight.position, movingObj.transform.rotation).transform.parent = gameObject.transform;
                }
            }
            else
            {
                if (!waterOrRoad)
                {
                    movingObj.GetComponent<MovingObjectBehaviour>().defaultSpeed = currentTruckSpeed;
                    Instantiate(movingObj, spawnPosLeft.position, movingObj.transform.rotation).transform.parent = gameObject.transform;
                }
                else
                {
                    movingObj.GetComponent<MovingObjectBehaviour>().defaultSpeed = currentLogSpeed;
                    Instantiate(movingObj, spawnPosLeft.position, movingObj.transform.rotation).transform.parent = gameObject.transform;
                }
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