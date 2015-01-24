using UnityEngine;
using System.Collections;
using Pathfinding;

public class Agent : MonoBehaviour
{
    public float AISpeed = 100.0f;
    public float NextWaypointDistance = 5;

    private Seeker _seeker;
    private CharacterController _controller;
    private Path _path;
    private int _currentWaypoint = 0;

    // Use this for initialization
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _controller = GetComponent<CharacterController>();
    }

    public void SetDestination(Vector3 destination)
    {
        _seeker.StartPath(transform.position, destination, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            _path = p;
            foreach(var pathnode in _path.path)
            {
                Debug.Log(_path.GetTraversalCost(pathnode));
            }
        }
        else
        {
            _path = null;
            Room room = GetCurrentRoom();
            Debug.Log("Currently in room " + (room == null), room);
            Debug.Log("Has door: " + room.HasDoor);
        }
        _currentWaypoint = 0;
    }

    void Update()
    {
        if (_path == null)
        {
            return;
        }

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            return;
        }

        //Direction to the next waypoint
        Vector3 nextWaypoint = new Vector3(_path.vectorPath[_currentWaypoint].x, transform.position.y, _path.vectorPath[_currentWaypoint].z);
        Vector3 dir = (nextWaypoint - transform.position).normalized;
        dir *= AISpeed * Time.deltaTime;
        _controller.Move(dir);

        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance(transform.position, nextWaypoint) < NextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }

    private Room GetCurrentRoom()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, 1 << Layers.Ground))
        {
            var room = hit.collider.gameObject.GetComponent<Room>();
            Debug.Log("raycast successful");
            return room;
        }
        else
        {
            return null;
        }
    }
}
