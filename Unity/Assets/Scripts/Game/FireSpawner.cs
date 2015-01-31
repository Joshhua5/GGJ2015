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
    
    private RoomManager roomManager;
	// Use this for initialization
	void Start () {
		roomManager = new RoomManager(rooms);
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
				Vector3 roomPos = rooms[roomId].transform.localPosition;  
				if (roomId > 8){
					//Debug.Log ("Room: "+roomId+" local pos: "+roomPos);
					//Debug.Log ("Romm pos: "+rooms[roomId].transform.position);
					//Debug.Log ("parent pos: "+rooms[roomId].transform.parent.position);
					roomPos.x = roomPos.x+rooms[roomId].transform.parent.position.x;
				}
				obj.transform.position = roomPos + ((Vector3.right + Vector3.forward) * 64);
				obj.transform.position = new Vector3(obj.transform.position.x, 0 , obj.transform.position.y);
				Room cr = roomManager.getRoom (obj);
				if (cr != null){
					rooms[roomId].GetComponentInParent<Room>().Fire = obj;
					Fire f = obj.GetComponentInParent<Fire>();
					if (cr.HasWater ()){
						Destroy(obj);
					}
					else {
						f.setRoom(cr);
						cr.fires.Push (obj);
					}
				}
				else {
					Destroy (obj);
					Debug.Log ("No room found for fire spawned from room: "+r.ToString ()+" So don't spawn child");
				}
			}
        }
	}
	
}
