using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerNode : MonoBehaviour {

	public List<PowerNode> children;
	public bool on = true;
	public bool connected = true;
	public bool consumesPower;
	public int roomNo;

    // Particle Effect
    public ParticleSystem triggerEffect;
    private ParticleSystem privateTriggerEffect;

	private PowerSystem ps;
	public bool parentConnected = true;

	// Use this for initialization
	void Start () { 
        if (triggerEffect != null)
            privateTriggerEffect = (ParticleSystem)Instantiate(triggerEffect);
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
	
	public PowerNode(int roomNo,bool cp){
		children = new List<PowerNode> ();
		this.roomNo = roomNo;
		consumesPower = cp;
	}

	public void setPowerSystem(PowerSystem ps){
		this.ps=ps;
		//Debug.Log ("connected to power system with " + ps.totalPower);
		foreach (PowerNode child in children) {
			child.setPowerSystem (ps);
		}
	}
	
	public void add(PowerNode child){
		children.Add (child);
		Debug.Log ("Child added");
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
	
	public bool connect(){
		parentConnected = true;
		if (!connected) {
			if (ps.hasPower () && on){ 
				connected = true;
				ps.usePower();
			}
		}return true;
	}
	
	public void disconnect(){
		parentConnected = false;
		if (connected) {
			if (isActive ()) ps.releasePower ();
			connected = false;
		}
	}

	public void updateChildren(){
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
		if (on) 
		{
			if (isActive ()) {
				ps.releasePower ();
				connected = false;
			}
			on = false;
		} 
		else 
		{
			on = true;
			if (parentConnected && ps.hasPower ())
			{
				ps.usePower ();
				connected = true;
			}
		}
        if (privateTriggerEffect != null)
            privateTriggerEffect.Play();

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
	/*
	 * public Color c1 = Color.white;
    public Color c2 = new Color(1, 1, 1, 0);
    void Start() {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(c1, c2);
    }*/
	public void drawLines(){
		foreach (PowerNode child in children) 
		{
			var go = new GameObject();
			var lr = go.AddComponent<LineRenderer>();
			//transform.position, c.transform.position
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, child.transform.position);
			lr.material = new Material(Shader.Find ("Particles/Additive"));
			lr.SetColors (Color.white,Color.black);
			lr.SetWidth (7.0f,5f);
			child.drawLines ();
		}
	}
}
