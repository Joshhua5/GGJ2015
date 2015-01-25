using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	private SkeletonAnimation _animation;

	// Use this for initialization
	void Start () {
		_animation = GetComponentInChildren<SkeletonAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetState(int power)
	{
		
		if (power == 0)
				_animation.state.SetAnimation (0, "Off", false);
		else if (power == 1)
				_animation.state.SetAnimation (0, "OnOne", false);
		else if (power == 2)
				_animation.state.SetAnimation (0, "OnTwo", false);
		else if (power == 3)
			_animation.state.SetAnimation (0, "OnThree", false);
		else if (power == 4)
			_animation.state.SetAnimation (0, "OnFour", false);
		else if (power == 5)
			_animation.state.SetAnimation (0, "OnFive", false);
		else if (power == 6)
			_animation.state.SetAnimation (0, "OnSix", false);
		else if (power == 7)
			_animation.state.SetAnimation (0, "OnSeven", false);
		else
			_animation.state.SetAnimation(0, "On", false);
			//_animation.state.SetAnimation(0, ("On"+power), false);
		
		_animation.Update();
		Debug.Log ("Set generator state");
	}
}
