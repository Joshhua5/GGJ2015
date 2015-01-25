using Pathfinding;
using UnityEngine;
using Spine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool Open { get; private set; }

    [SerializeField]
    private uint chargeTime = 5;

    [SerializeField]
    private AudioClip _openSound;
    
    [SerializeField]
    private AudioClip _closeSound;

    [SerializeField]
    private int _doorStartingHP = 100;

    public float HP { get; private set; }

    private float charge;
    private Room room;
    private SkeletonAnimation _animation;
    private bool charging;
    Bounds bounds;

    public delegate void DoorStateChanged(bool isOpen);
    public event DoorStateChanged OnDoorStateChanged;

    public void Start()
    {
        HP = _doorStartingHP;
        _animation = GetComponentInChildren<SkeletonAnimation>();
        room = GetComponentInParent<Room>();
        Open = false;
        bounds = collider.bounds;
        SetState(Open);

        charging = false;
    }

    public void SetState(bool open)
    {
        if (HP <= 0) open = true; // override, can't close a broken door
        Open = open;

        if (Open)
        {
            _animation.state.SetAnimation(0, "open", false);
            AudioSource.PlayClipAtPoint(_openSound, Camera.main.transform.position);
        }
        else
        {
            _animation.state.SetAnimation(0, "close", false);
            AudioSource.PlayClipAtPoint(_closeSound, Camera.main.transform.position);
        }
        _animation.Update();

        if (OnDoorStateChanged != null)
            OnDoorStateChanged(open);
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

    public void TakeDamage(float hp)
    {
        HP -= hp;
        if (HP <= 0)
        {
            SetState(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (charging) 
            charge += Time.deltaTime; 
        else if(charge > 0)
            charge -= Time.deltaTime;
        if (charge >= chargeTime)
        {
            charging = false;
            if(room.HasPower())
               SetState(true);
        }
    } 
}
