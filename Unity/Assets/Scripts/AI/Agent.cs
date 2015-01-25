using UnityEngine;
using Spine;
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
    private SkeletonAnimation _animation;
    private Vector3 _destination;

    [SerializeField]
    private GameObject _animationHolder;

    [SerializeField]
    private float _initialHealth = 100;

    [SerializeField]
    private float _firePenalty = 10; 
    private float _health;

    private float _speed;

    // Raycast details
    private bool _raycastHit;
    private RaycastHit _hit;

    // Use this for initialization
    void Awake()
    {
        _speed = Random.Range(AISpeed - 20, AISpeed + 20);
        _seeker = GetComponent<Seeker>();
        _controller = GetComponent<CharacterController>();
        _animation = GetComponentInChildren<SkeletonAnimation>(); 
    }

    public void SetDestination(Vector3 destination)
    {
        _seeker.StartPath(transform.position, destination, OnPathComplete);
        _destination = destination;
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
        }
        else
        {
            _path = null;
            Room room = GetCurrentRoom();
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
        dir *= _speed;
        _controller.SimpleMove(dir);

        _animationHolder.transform.LookAt(_animationHolder.transform.position + dir);

        if (_path.IsDone())
            _animation.state.SetAnimation(0, "walk", true);
        else 
            _animation.state.SetAnimation(0, "standing", true);

        _animation.Update();

        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance(transform.position, nextWaypoint) < NextWaypointDistance)
        {
            _currentWaypoint++;
        } 
        else if (Vector3.Distance(transform.position, nextWaypoint) > NextWaypointDistance  * 4)
        {
            SetDestination(_destination);
            _path = null;
            _currentWaypoint = 0;
        }


        Ray ray = new Ray(transform.position, Vector3.down);  

        _raycastHit = Physics.Raycast(ray, out _hit, 100, 1 << Layers.Ground);
        if (_raycastHit)
            // Check if object is fire
            if (_hit.collider.gameObject.GetComponent<Fire>() != null)
                _health -= _firePenalty * Time.deltaTime;

        if (_health < 0)
            // Dead
            DestroyObject(this);

    }

    private Room GetCurrentRoom()
    { 
        if (_raycastHit)
            return _hit.collider.gameObject.GetComponent<Room>();
        else 
            return null; 
    }
}
