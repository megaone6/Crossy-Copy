using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{
    public float defaultSpeed;
    public float speedDifference;
    private float minSpeedDifference;
    private float maxSpeedDifference;

    private void Start()
    {
        minSpeedDifference = 0.8f;
        maxSpeedDifference = 2.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        speedDifference = Random.Range(minSpeedDifference, maxSpeedDifference);
        if (gameObject.name.Contains("Car"))
            transform.Translate(Vector3.left * defaultSpeed * speedDifference * Time.deltaTime);
        else if (gameObject.name.Contains("Truck"))
        {
            transform.Translate(Vector3.right * defaultSpeed * speedDifference * Time.deltaTime);
        }
    }

    private void OnCollisionExit()
    {
        Destroy(gameObject);
    }
}
