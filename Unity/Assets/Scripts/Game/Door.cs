using Pathfinding;
using UnityEngine;
using Spine;

public class Door : MonoBehaviour
{
    public bool Open { get; private set; }
    private SkeletonAnimation _animation;
    

    Bounds bounds;

    public void Start()
    {
        _animation = GetComponentInChildren<SkeletonAnimation>();

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

        if (Open)
            _animation.state.SetAnimation(0, "open", false);
        else
            _animation.state.SetAnimation(0, "Close", false);

        _animation.Update();
        
        //AstarPath.active.UpdateGraphs(guo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
