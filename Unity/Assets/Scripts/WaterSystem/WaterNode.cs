using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterNode : MonoBehaviour {
	
	public List<WaterNode> children;
	public bool on = true;
	public bool connected = true;
	public bool consumesPower;
	public int roomNo;
	
	// Particle Effect
	public ParticleSystem triggerEffect;
	private ParticleSystem _triggerEffect;
	
	private WaterSystem ws;
	public bool parentConnected = true;
	
	// Use this for initialization
	void Start () {
		if (triggerEffect != null)
		{
			_triggerEffect = (ParticleSystem)Instantiate(triggerEffect);
			_triggerEffect.Stop();
			_triggerEffect.transform.position = this.transform.position;
		}
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
	
	public WaterNode(int roomNo,bool cp){
		children = new List<WaterNode> ();
		this.roomNo = roomNo;
		consumesPower = cp;
	}
	
	public void setWaterSystem(WaterSystem ws){
		this.ws=ws;
		//Debug.Log ("connected to power system with " + ps.totalPower);
		foreach (WaterNode child in children) {
			child.setWaterSystem (ws);
		}
	}
	
	public void add(WaterNode child){
		children.Add (child);
		Debug.Log ("Child added");
	}
	
	public List<WaterNode> getChildren(){
		return children;
	}
	
	public int getCurrentUsage(){
		int total = on ? 1 : 0;
		foreach (WaterNode node in children) {
			total += node.getCurrentUsage ();
		}
		return total;
	}
	
	public bool connect(){
		parentConnected = true;
		if (!connected) {
			if (ws.hasWater () && on){ 
				connected = true;
				ws.useWater();
			}
		}return true;
	}
	
	public void disconnect(){
		parentConnected = false;
		if (connected) {
			if (isActive ()) ws.releaseWater ();
			connected = false;
		}
	}
	
	public void updateChildren(){
		if (isActive ()) connectChildren ();
		else disconnectChildren ();
	}
	
	private void disconnectChildren(){
		Queue<WaterNode> toVisit = new Queue<WaterNode>();
		addAll (toVisit, children);
		while (toVisit.Count > 0) {
			WaterNode child = toVisit.Dequeue();
			child.disconnect ();
			if (!child.isActive ()){
				addAll(toVisit, child.getChildren());
			}
		}
	}
	
	private void addAll(Queue<WaterNode> q,List<WaterNode> vals){
		foreach (WaterNode v in vals) {
			q.Enqueue (v);
		}
	}
	
	private void connectChildren(){
		Queue<WaterNode> toVisit = new Queue<WaterNode>();
		addAll (toVisit, children);
		while (toVisit.Count > 0) {
			WaterNode child = toVisit.Dequeue();
			child.connect ();
			if (child.isActive ()){
				addAll(toVisit, child.getChildren());
			}
		}
	}
	
	public void toggle(){
		if (on) 
		{
			// Should have turn off sound
			if (isActive ()) {
				ws.releaseWater ();
				connected = false;
			}
			on = false;
		} 
		else 
		{ 
			on = true;
			if (parentConnected && ws.hasWater ())
			{            
				// Only spark if being turned on, 
				if (_triggerEffect != null)
					_triggerEffect.Play();
				ws.useWater ();
				connected = true;
			}
		}
		
		//this.audio.Play ();
		//Debug.Log (""+roomNo+": toggle pressed. on="+on);
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
			foreach (WaterNode c in children) {
				s += c.ToString () + ",";
			}
			s += "]";
		}
		return s;
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		foreach (WaterNode c in children) {
			Gizmos.DrawLine(transform.position, c.transform.position);
		}
	}

	public void drawLines(){
		foreach (WaterNode child in children) 
		{
			var go = new GameObject();
			var lr = go.AddComponent<LineRenderer>();
			//transform.position, c.transform.position
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, child.transform.position);
			lr.material = new Material(Shader.Find ("Particles/Additive"));
			lr.SetColors (Color.blue,Color.cyan);
			lr.SetWidth (7.0f,5f);
			child.drawLines ();
		}
	}
}
