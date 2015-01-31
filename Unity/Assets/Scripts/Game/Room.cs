using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Door[] _doors;
    
    [SerializeField]
    private int id;

	[SerializeField]
	private bool waterOn = false;

	public PowerNode powerNode;
	
	public Stack<GameObject> fires = new Stack<GameObject>();
	
	//public List<GameObject> fires = new List<GameObject>();
	[SerializeField]
	private float waterRate = 1;
	float charge=0;

    public GameObject Fire;

    [SerializeField]
    private Light _roomLight;

	public bool HasWater(){
		return waterOn;
	}
	
	public void setHasWater(bool state){
		waterOn = state;
	}

    public bool HasDoor
    {
        get
        {
            return _doors != null;
        }
    }

    public bool HasPower() { return powerNode.isActive(); }

    void Update()
    {
        _roomLight.enabled = powerNode.isActive();
		charge += Time.deltaTime;
		if (HasWater() && charge > waterRate){ 
			killRandomFire();
			charge=0;
		}
    }
    
    private void killRandomFire(){
		if (fires.Count() > 0){
			Debug.Log ("Num fires: "+fires.Count());
			GameObject f = fires.Pop();
			Destroy (f);
			Debug.Log ("updated Num fires: "+fires.Count());
			//System.Random rnd = new System.Random();
			//int index = rnd.Next(fires.Count()-1);
			//int index = fires.Count()-1;
			//GameObject f = fires.ElementAt(index);
			//fires.RemoveAt(index);
			//Destroy (f);
			//Debug.Log ("Destroyed fire "+index+" fires remaining: "+fires.Count());
		}
    }

    public Door FirstDoor
    {
        get
        {
            return _doors[0];
        }
    } 
    
	public override string ToString(){
		String s = "Room ID: "+this.id+" water? "+HasWater()+" power? "+powerNode.isActive();
		return s;
	}
}	