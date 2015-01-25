using UnityEngine;
using System.Collections;

public class FlashingExitLight : MonoBehaviour {

	public GameObject emergencyLightFlash;
	private bool lightOn;

	void Start () {
	
		emergencyLightFlash.gameObject.SetActive (true);
		lightOn = true;

	}
	

	void FixedUpdate () {
	
		if (lightOn) {
			StartCoroutine(lightFlash(0.5F));
	}

	}

IEnumerator lightFlash(float flashInterval){

		lightOn = false;
		emergencyLightFlash.gameObject.SetActive (false);
		yield return new WaitForSeconds (flashInterval);
		emergencyLightFlash.gameObject.SetActive (true);
		yield return new WaitForSeconds (flashInterval);
		lightOn = true;
	}

}
