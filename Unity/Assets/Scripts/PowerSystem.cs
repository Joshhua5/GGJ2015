using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerSystem : MonoBehaviour {
	public PowerNode root;
	public int totalPower;
	public int availablePower;

	// Use this for initialization
	void Start () {
		//totalPower = 10;
		//availablePower = 10;
		/*TreeNode root = new TreeNode(1,0,0,false);
		TreeNode a = new TreeNode (2, 0, 0,true);
		TreeNode b = new TreeNode (3, 0, 0,true);
		TreeNode c = new TreeNode (4, 0, 0,true);
		root.toggle ();
		a.toggle ();
		c.toggle ();

		a.add (b);
		a.add (c);
		root.add (a);
		Debug.Log(root.ToString ());
		Debug.Log (root.getCurrentUsage ());
		a.disconnect ();
		Debug.Log (root.ToString ());
		a.toggle ();
		a.connect ();
		Debug.Log (root.ToString ());
		a.toggle ();*/
		root.setPowerSystem (this);
		Debug.Log(root.ToString ());
		//root.toggle ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updatePowerSpread()
	{
		Debug.Log ("update children of root");
		root.updateChildren ();
	}

	public bool usePower(){
		if (hasPower ()) {
			availablePower--;
			Debug.Log ("Use 1 unit. Power remaining = "+availablePower);
			return true;
		}
		return false;
	}

	public void releasePower(){
		if (availablePower < totalPower) 
		{
			availablePower++;
			Debug.Log ("Release 1 unit. Power remaining = " + availablePower);
			if (availablePower == 1) updatePowerSpread ();
		}
	}

	int getTotalCapacity(){
		return totalPower;
	}

	int getCurrentUsage(){
		return (totalPower - availablePower);
	}

	public bool hasPower(){ return availablePower > 0;}


}
