using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerNode : MonoBehaviour {

	public List<PowerNode> children;
	public bool on = true;
	public bool connected = true;
	public bool consumesPower;
	public int roomNo;
	public Material materialOn;
	public Material materialOff;
	public bool parentConnected = true;

    // Particle Effect
    public ParticleSystem triggerEffect;
    private ParticleSystem _triggerEffect;

	private PowerSystem ps;
	private LineRenderer lineIn = null;

	private static Color _lineOn = new Color(0,237,214);
	private static Color _lineOff = Color.gray;

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
		if (isActive ()) {
						setOnTexture ();
						lineOn ();
		} 
		else 
		{
				setOffTexture ();
				lineOff ();
		}
	}

	private void setOffTexture()
	{
		renderer.material = materialOff;
	}

	private void setOnTexture()
	{
		renderer.material = materialOn;
	}
	
	void OnMouseDown(){
		toggle ();
	}

	public PowerNode(int roomNo,bool cp){
		children = new List<PowerNode> ();
		this.roomNo = roomNo;
		consumesPower = cp;
	}

	public void setPowerSystem(PowerSystem ps){
		this.ps=ps;
		foreach (PowerNode child in children) {
			child.setPowerSystem (ps);
		}
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
            // Should have turn off sound
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
                 // Only spark if being turned on, 
                if (_triggerEffect != null)
                    _triggerEffect.Play();
				ps.usePower ();
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

	public void setLineIn(LineRenderer line)
	{
		this.lineIn = line;
	}

	private void lineOn(){
		if (lineIn != null) {
			Debug.Log ("switch line on");
						lineIn.SetColors (_lineOn, _lineOn);

				}
	}

	private void lineOff(){
		if (lineIn != null) lineIn.SetColors (_lineOff, _lineOff);
	}

	public void drawLines(){
		foreach (PowerNode child in children) 
		{
			var go = new GameObject();
			var lr = go.AddComponent<LineRenderer>();
			//transform.position, c.transform.position
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, child.transform.position);
			lr.material = new Material(Shader.Find ("Particles/Additive"));
			if (isActive () && child.isActive ()) lr.SetColors (_lineOn,_lineOn);
			else lr.SetColors (_lineOff,_lineOff);
			lr.SetWidth (3.0f,3f);
			child.setLineIn (lr);
			child.drawLines ();
		}
	}
}
