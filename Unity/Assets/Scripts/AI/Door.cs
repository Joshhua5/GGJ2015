using Pathfinding;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Open { get; private set; }

    Bounds bounds;

    public void Start()
    {
        Open = false;
        bounds = collider.bounds;
        SetState(Open);
    }

    public void SetState(bool open)
    {
        Open = open;

        var guo = new GraphUpdateObject(bounds);
        guo.modifyWalkability = true;
        guo.setWalkability = Open;
        guo.updatePhysics = false;

        AstarPath.active.UpdateGraphs(guo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
