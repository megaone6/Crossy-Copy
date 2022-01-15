using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 restorePosition;
    public float delay;
    public float timestamp;
    private bool dead;

    private void Start()
    {
        restorePosition = transform.position;
        delay = 2;
        timestamp = Time.realtimeSinceStartup + delay;
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead)
        {
            enabled = false;
        }
        if (Time.realtimeSinceStartup > timestamp)
            transform.position += Vector3.right * Time.deltaTime * 2f;
    }

    public void FollowPlayerForward()
    {
        transform.position += Vector3.left * 0.3f;
        restorePosition = transform.position;
    }

    public void FollowPlayerLeft()
    {
        transform.position += Vector3.forward * 1;
        restorePosition = transform.position;
    }

    public void FollowPlayerRight()
    {
        transform.position += Vector3.back * 1;
        restorePosition = transform.position;
    }

    public void RestorePosition()
    {
        transform.position = restorePosition;
    }

    public void SetDeath()
    {
        dead = true;
    }
}
