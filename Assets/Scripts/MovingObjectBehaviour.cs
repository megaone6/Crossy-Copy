using System.Collections;
using UnityEngine;

public class MovingObjectBehaviour : MonoBehaviour
{
    public float defaultSpeed;
    public float speedDifference;
    private float minSpeedDifference;
    private float maxSpeedDifference;
    private Rigidbody rb;
    private HingeJoint tempHinge;

    private void Start()
    {
        minSpeedDifference = 0.8f;
        maxSpeedDifference = 1.2f;
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(1, 0, 0);
        if (!gameObject.name.Contains("Log"))
            speedDifference = Random.Range(minSpeedDifference, maxSpeedDifference);
        else
            speedDifference = 1f;
        StartCoroutine(GenerateSpeedDifference());
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 tmp;
        if (gameObject.name.Contains("Car") || gameObject.name.Contains("LogRight"))
            tmp = new Vector3(0, 0, 1);
        else
            tmp = new Vector3(0, 0, -1);
        tmp = tmp.normalized;
        rb.MovePosition(transform.position + (tmp * defaultSpeed * speedDifference * Time.deltaTime*2));
    }

    private IEnumerator GenerateSpeedDifference()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (!gameObject.name.Contains("Log"))
                speedDifference = Random.Range(minSpeedDifference, maxSpeedDifference);
            else
                speedDifference = 1f;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.name.Contains("Player") && gameObject.name.Contains("Log"))
        {
            coll.gameObject.transform.position = gameObject.transform.position + new Vector3(0,0.3f,0);
            tempHinge = coll.gameObject.AddComponent<HingeJoint>();
            tempHinge.connectedBody = gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit()
    {
        Destroy(gameObject);
    }

    private void OnCollisionExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player") && gameObject.name.Contains("Log"))
        {
            Destroy(other.gameObject.GetComponent<HingeJoint>());
        }
            
    }
}
