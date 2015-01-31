using UnityEngine;
using System.Collections;

public class WaterPump : MonoBehaviour
{

    private SkeletonAnimation _animation;

    // Use this for initialization
    void Start()
    {
        _animation = GetComponentInChildren<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetState(int water)
    {

        if (water == 0)
            _animation.state.SetAnimation(0, "Off", true);
        else if (water == 1)
            _animation.state.SetAnimation(0, "OnOne", true);
        else if (water == 2)
            _animation.state.SetAnimation(0, "OnTwo", true);
        else if (water == 3)
            _animation.state.SetAnimation(0, "OnThree", true);
        else if (water == 4)
            _animation.state.SetAnimation(0, "OnFour", true);
        else if (water == 5)
            _animation.state.SetAnimation(0, "OnFive", true);
        else if (water == 6)
            _animation.state.SetAnimation(0, "OnSix", true);
        else if (water == 7)
            _animation.state.SetAnimation(0, "OnSeven", true);
        else
            _animation.state.SetAnimation(0, "On", true);
        //_animation.state.SetAnimation(0, ("On"+power), false);

        _animation.Update();
    }
}
