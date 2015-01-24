using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerSystem : MonoBehaviour {
	public PowerNode root;
	// Use this for initialization
	void Start () {
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
		Debug.Log(root.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//int getTotalCapacity(){}

	//int getCurrentUsage(){}




}
