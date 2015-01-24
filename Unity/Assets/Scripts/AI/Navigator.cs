using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    [SerializeField]
    Transform destination;

    Agent _agent;

    // Use this for initialization
    void Start()
    {
        _agent = GetComponent<Agent>();
        _agent.SetDestination(destination.position);
        AstarPath.OnGraphsUpdated += OnGraphsUpdated;
    }

    void OnGraphsUpdated(AstarPath script)
    {
        _agent.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
