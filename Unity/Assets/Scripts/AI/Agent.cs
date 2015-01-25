using UnityEngine;
using Spine;
using System.Collections;
using Pathfinding;

public class Agent : MonoBehaviour
{
    public float AISpeed = 100.0f;
    public float NextWaypointDistance = 5;
    public float FightDoorDistance = 16;

    private enum AgentState
    {
        Idle,
        MovingToAttackDoor,
        AttackingDoor,
        MovingToAttackFire,
        AttackingFire,
        Running
    };
    private AgentState _state;

    private Seeker _seeker;
    private CharacterController _controller;
    private Path _path;
    private int _currentWaypoint = 0;
    private SkeletonAnimation _animation;
    private Vector3 _destination;
    private Vector3 _doorDestination;

    [SerializeField]
    private GameObject _animationHolder;

    [SerializeField]
    private float _initialHealth = 100;

    [SerializeField]
    private AudioSource _deathSound;

    [SerializeField]
    private float _firePenalty = 10;
    private float _health;

    private float _speed;
    private Door _currentDoor;

    // Raycast details
    private bool _raycastHit;
    private RaycastHit _hit;

    // Use this for initialization
    void Awake()
    {
        _state = AgentState.Idle;
        _speed = Random.Range(AISpeed - 20, AISpeed + 20);
        _seeker = GetComponent<Seeker>();
        _controller = GetComponent<CharacterController>();
        _animation = GetComponentInChildren<SkeletonAnimation>();
    }

    public void SetDestination(Vector3 destination)
    {
        _destination = destination;
        Room room = GetCurrentRoom();

        if (room != null && room.HasDoor && !room.FirstDoor.Open)
        {
            _currentDoor = room.FirstDoor;
            _seeker.StartPath(transform.position, _currentDoor.transform.position, OnDoorPathComplete);
            _doorDestination = _currentDoor.transform.position;
            room.FirstDoor.OnDoorStateChanged += FirstDoor_OnDoorStateChanged;
        }
        else
        {
            _seeker.StartPath(transform.position, destination, OnPathComplete);
        }
    }

    void OnDestroy()
    {
        if (_currentDoor != null)
            _currentDoor.OnDoorStateChanged -= FirstDoor_OnDoorStateChanged;
    }

    void FirstDoor_OnDoorStateChanged(bool isOpen)
    {
        SetDestination(_destination);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _state = AgentState.Running;
        }
        else
        {
            _animation.state.SetAnimation(0, "standing", true);
            _path = null;
        }
        _currentWaypoint = 0;
    }

    void OnDoorPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _state = AgentState.MovingToAttackDoor;
        }
        else
        {
            _animation.state.SetAnimation(0, "standing", true);
            _path = null;
            Debug.Log("Agent doesn't have an action");
        }
        _currentWaypoint = 0;
    }

    void Update()
    {
        if (_path == null)
        {
            if (_state == AgentState.AttackingDoor)
            {
                Room room = GetCurrentRoom();
                if (room != null && room.HasDoor && !room.FirstDoor.Open)
                {
                    Door door = room.FirstDoor;
                    door.TakeDamage(Time.deltaTime);
                }
            }

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
        if (_state == AgentState.MovingToAttackDoor)
        {
            if (Vector3.Distance(transform.position, _doorDestination) < FightDoorDistance)
            {
                _path = null;
                _currentWaypoint = 0;
                _state = AgentState.AttackingDoor;
                _animation.state.SetAnimation(0, "standing", true);
            }
            else if (Vector3.Distance(transform.position, nextWaypoint) < NextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }
        else if (_state == AgentState.Running)
        {
            if (Vector3.Distance(transform.position, nextWaypoint) < NextWaypointDistance)
            {
                _currentWaypoint++;
            }
            else if (Vector3.Distance(transform.position, nextWaypoint) > NextWaypointDistance * 4)
            {
                SetDestination(_destination);
                _path = null;
                _currentWaypoint = 0;
            }
        }

        Ray ray = new Ray(transform.position, Vector3.down);

        _raycastHit = Physics.Raycast(ray, out _hit, 100, 1 << Layers.Ground);
        if (_raycastHit)
            // Check if object is fire
            if (_hit.collider.gameObject.GetComponent<Fire>() != null)
                _health -= _firePenalty * Time.deltaTime;

        if (_health < 0)
        {
            // Dead
            DestroyObject(this);
            _deathSound.Play();
        }

    }

    private Room GetCurrentRoom()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, 1 << Layers.Ground))
            return hit.collider.transform.parent.parent.gameObject.GetComponent<Room>();
        else
            return null;
    }
}
