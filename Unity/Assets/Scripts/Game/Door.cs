using Pathfinding;
using UnityEngine;
using Spine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool Open { get; private set; }

    private float charge;
    private Room room;
    private SkeletonAnimation _animation;
    private bool charging;
    Bounds bounds;

    public void Start()
    {
        _animation = GetComponentInChildren<SkeletonAnimation>();
        room = GetComponentInParent<Room>();
        Open = false;
        bounds = collider.bounds;
        SetState(Open);

        charging = false;
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
        
        AstarPath.active.UpdateGraphs(guo);
    }

    void OnMouseDown() {
        if (room.HasPower())
            if (Open || charging)
            {
                charging = false;
                SetState(false);
            }
            else
                charging = true;
        // Possible responce such as a red light if the door fails.
    } 

    // Update is called once per frame
    void Update()
    {
        if (charging) 
            charge += Time.deltaTime; 
        else if(charge > 0)
            charge -= Time.deltaTime;
        if (charge >= 5)
        {
            charging = false;
            if(room.HasPower())
               SetState(true);
        }
    } 
}
