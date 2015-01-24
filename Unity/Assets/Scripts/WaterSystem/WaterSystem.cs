using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterSystem : MonoBehaviour {
	public WaterNode root;
	public int totalWater;
	public int availableWater;
	
	// Use this for initialization
	void Start () {
		root.setWaterSystem (this);
		root.drawLines ();
		Debug.Log(root.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void updateWaterSpread()
	{
		Debug.Log ("update children of root");
		root.updateChildren ();
	}
	
	public bool useWater(){
		if (hasWater ()) {
			availableWater--;
			Debug.Log ("Use 1 unit. Power remaining = "+availableWater);
			return true;
		}
		return false;
	}
	
	public void releaseWater(){
		if (availableWater < totalWater) 
		{
			availableWater++;
			Debug.Log ("Release 1 unit. Water remaining = " + availableWater);
			if (availableWater == 1) updateWaterSpread ();
		}
	}
	
	int getTotalCapacity(){
		return totalWater;
	}
	
	int getCurrentUsage(){
		return (totalWater - availableWater);
	}
	
	public bool hasWater(){ return availableWater > 0;}
	
	
}

