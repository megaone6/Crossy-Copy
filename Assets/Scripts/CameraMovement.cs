using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 restorePosition;
    public float delay;
    public float timestamp;
    private bool dead;
    private float dist;
    public String followOnLog;

    private void Start()
    {
        restorePosition = transform.position;
        delay = 3;
        timestamp = Time.realtimeSinceStartup + delay;
        followOnLog = "none";
        dist = 22.36f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead)
            enabled = false;
        if (Time.realtimeSinceStartup > timestamp)
            transform.position += Vector3.right * Time.deltaTime * 2f * dist/15f;
        if (followOnLog == "left")
            transform.position += Vector3.back * Time.deltaTime * 1.2f;
        else if (followOnLog == "right")
            transform.position += Vector3.forward * Time.deltaTime * 1.2f;
    }

    public void FollowPlayerForward(float dist)
    {
        this.dist = dist;
        transform.position += Vector3.left;
        restorePosition = transform.position;
    }

    public void FollowPlayerLeft(float dist)
    {
        this.dist = dist;
        transform.position += Vector3.forward;
        restorePosition = transform.position;
    }

    public void FollowPlayerRight(float dist)
    {
        this.dist = dist;
        transform.position += Vector3.back;
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
