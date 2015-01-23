using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerNode : MonoBehaviour {
	public List<PowerNode> children;
	public bool on = false;
	public bool connected = true;
	public bool consumesPower;
	public int roomNo;
	public float x;
	public float y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//bool consumesEnergy = false; 
	
	public PowerNode(int roomNo,float x, float y,bool cp){
		children = new List<PowerNode> ();
		this.roomNo = roomNo;
		this.x=x;
		this.y=y;
		consumesPower = cp;
	}
	
	public void add(PowerNode child){
		children.Add (child);
	}
	
	public List<PowerNode> getChildren(){
		return children;
	}
	
	public int getCurrentUsage(){
		int total = on ? 1 : 0;
		foreach (PowerNode node in children) {
			total += node.getCurrentUsage ();
		}
		return total;
	}
	
	public void connect(){
		if (!connected) {
			connected = true;
			updateChildren ();
		}
	}
	
	public void disconnect(){
		if (connected) {
			connected = false;
			updateChildren ();
		}
	}
	
	private void updateChildren(){
		foreach (PowerNode child in children) {
			if (isActive ()) child.connect ();
			else child.disconnect ();
		}
	}
	
	public void toggle(){
		on = on ? false : true;
		updateChildren ();
	}
	
	public bool isActive(){
		return connected && on;
	}
	
	public bool isOn(){
		return on;
	}
	
	public bool isConnected(){
		return connected;
	}
	
	public override string ToString(){
		string s = "No: " + roomNo + " on: " + on + " connected: " + connected + " active: " + isActive ();
		if (children.Count > 0) {
			s += " [";
			foreach (PowerNode c in children) {
				s += c.ToString () + ",";
			}
			s += "]";
		}
		return s;
	}
}
