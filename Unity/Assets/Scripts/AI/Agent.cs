using UnityEngine;
using System.Collections;
using Pathfinding;

public class Agent : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDestination(Vector3 destination)
    {
        var seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, destination, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
    }
}
