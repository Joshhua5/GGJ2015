using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	PrefabManager prefabManager;
	public GameObject particleSpawn;

	void Start () {
		prefabManager = GameObject.FindObjectOfType<PrefabManager> ();
		particleSpawn = null;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Input.GetKeyDown(KeyCode.Q)){
			particleSpawn = prefabManager.fireSpawn;
				}

		else if (Input.GetKeyDown(KeyCode.A)){
			particleSpawn = prefabManager.smokeSpawn;
		}

		else if (Input.GetKeyDown(KeyCode.Z)){
			particleSpawn = prefabManager.sparkSpawn;
		}

		else if (Input.GetKeyDown(KeyCode.W)){
			particleSpawn = prefabManager.spraySpawn;
		}

		else if (Input.GetKeyDown(KeyCode.S)){
			particleSpawn = prefabManager.waterSpawn;
		}

		/*switch (Input.GetKeyDown) {
				case(KeyCode.Q):
						particleSpawn = prefabManager.fireSpawn;
						break;
				case(KeyCode.A):
						particleSpawn = prefabManager.smokeSpawn;
						break;
				case(KeyCode.Z):
						particleSpawn = prefabManager.sparkSpawn;
						break;
				case(KeyCode.W):
						particleSpawn = prefabManager.spraySpawn;
						break;
				case(KeyCode.S):
						particleSpawn = prefabManager.waterSpawn;
						break;
						*/
				
				
		if(Physics.Raycast(ray, out hit))
		{
			if(Input.GetMouseButtonDown(0))
			{
			Instantiate (particleSpawn, hit.transform.position, Quaternion.identity);
			}

	}
}
}

