using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerNode : MonoBehaviour {
	public List<PowerNode> children;
	public bool on = false;
	public bool connected = true;
	public bool consumesPower;
	public int roomNo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive ()) this.renderer.material.color = Color.green;
		else this.renderer.material.color = Color.blue;
	}

	void OnMouseDown(){
		toggle ();
	}

	//bool consumesEnergy = false; 
	
	public PowerNode(int roomNo, bool cp){
		children = new List<PowerNode> ();
		this.roomNo = roomNo;
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
		}
	}
	
	public void disconnect(){
		if (connected) {
			connected = false;
		}
	}

	private void updateChildren(){
		if (isActive ()) connectChildren ();
		else disconnectChildren ();
	}

	private void disconnectChildren(){
		Queue<PowerNode> toVisit = new Queue<PowerNode>();
		addAll (toVisit, children);
		while (toVisit.Count > 0) {
			PowerNode child = toVisit.Dequeue();
			child.disconnect ();
			if (!child.isActive ()){
				addAll(toVisit, child.getChildren());
			}
		}
	}

	private void addAll(Queue<PowerNode> q,List<PowerNode> vals){
		foreach (PowerNode v in vals) {
			q.Enqueue (v);
		}
	}

	private void connectChildren(){
		Queue<PowerNode> toVisit = new Queue<PowerNode>();
		addAll (toVisit, children);
		while (toVisit.Count > 0) {
			PowerNode child = toVisit.Dequeue();
			child.connect ();
			if (child.isActive ()){
				addAll(toVisit, child.getChildren());
			}
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

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		foreach (PowerNode c in children) {
			Gizmos.DrawLine(transform.position, c.transform.position);
		}
	}

}
