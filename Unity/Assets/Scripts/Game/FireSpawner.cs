using UnityEngine;
using System.Collections;

public class FireSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject[] rooms;
    [SerializeField]
    private float _spawnRate = 5;
    private float _charge = 0;

    [SerializeField]
    private GameObject fire;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _charge += Time.deltaTime;
	    if(_charge > _spawnRate){
            int roomId = Random.Range(0, rooms.Length);
			Room r = rooms[roomId].GetComponentInParent<Room>();
			_charge = 0;
			if (!r.HasWater()){
            	// Spawn fire
            	GameObject obj = (GameObject)Instantiate(fire); 
            	obj.transform.position = rooms[roomId].transform.localPosition + ((Vector3.right + Vector3.forward) * 64);
            	obj.transform.position = new Vector3(obj.transform.position.x, 0 , obj.transform.position.y);
            	rooms[roomId].GetComponentInParent<Room>().Fire = obj;
				obj.GetComponentInParent<Fire>().setRoom(r);
			}
        }
	}
}
