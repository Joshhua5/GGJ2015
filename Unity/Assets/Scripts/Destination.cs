using UnityEngine;
using System.Collections;

public class Destination : MonoBehaviour {

	public int score = 0;

	// Use this for initialization
	void Start () {
		Debug.Log ("Destination is at " + transform.position.x + " " + transform.position.y + " " + transform.position.z );
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log ( "Destination collision with" + collision.collider.GetType() );

		Destroy(collision.collider);
		score += 1;
		Debug.Log ("Agent reached the exit. Score: " + score );
	}

	// Update is called once per frame
	void Update () {
	
	}
}
