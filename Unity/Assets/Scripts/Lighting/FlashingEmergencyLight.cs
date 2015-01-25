using UnityEngine;
using System.Collections;

public class FlashingEmergencyLight : MonoBehaviour {

	public GameObject emergencyLightFlash;
	private bool lightOn;
	private float lightFlashInterval;

	void Start () {
	
		emergencyLightFlash.gameObject.SetActive (true);
		lightOn = true;

	}
	

	void FixedUpdate () {
	
		if (lightOn) {
			lightFlashInterval = Random.Range (0.1F, 0.5F);
			StartCoroutine(lightFlash(lightFlashInterval));
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
