using UnityEngine;
using System.Collections;

public class WaterSpawner : MonoBehaviour {
	public GameObject water;
	// Use this for initialization
	void Start () {
		Instantiate (water);  
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
