using UnityEngine;
using System.Collections;

public class FlashingEmergencyLight : MonoBehaviour {

	public GameObject emergencyLightFlash;
	private bool lightOn;

	void Start () {
	
		emergencyLightFlash.gameObject.SetActive (true);
		lightOn = true;

	}
	

	void FixedUpdate () {
	
		if (lightOn) {
			StartCoroutine(lightFlash(1));
	}

	}

IEnumerator lightFlash(float flashInterval){

		lightOn = false;
		emergencyLightFlash.gameObject.SetActive (false);
		Debug.Log ("Light Off");
		yield return new WaitForSeconds (flashInterval);
		emergencyLightFlash.gameObject.SetActive (true);
		Debug.Log ("Light Off");
		yield return new WaitForSeconds (flashInterval);
		lightOn = true;
	}

}
