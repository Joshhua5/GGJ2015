using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour {

	[SerializeField]
	private GameObject[] rooms;
	
	public RoomManager(GameObject[] rooms){
		this.rooms=rooms;
	}
	
	public Room getRoom(GameObject fire){
		Vector3 p1 = fire.transform.position;
		Vector3 s1 = fire.transform.localScale;
		//Debug.Log("FPos: "+p1+" FScale: "+s1);
		foreach (GameObject robj in rooms){
			Vector3 p2 = robj.transform.position;
			Vector3 s2 = robj.transform.localScale;
			//Debug.Log("FPos: "+p2+" FScale: "+s2);
			if (intersects (p1,s1,p2,s2))
				return robj.GetComponentInParent<Room>();
		}
		//Debug.Log ("Fire did not intersect any room renderer");
		
		return null;
	}
	
	/*public Vector3 getRoomPosition(GameObject floorPrefab){
		Vector3 fp = floorPrefab.transform.position;
		Vector3 pp = floorPrefab.GetComponentInParent<Transform>().position;
		Vector3 sum = new Vector3(fp.x+pp.x,fp.y+pp.y,fp.z+pp.z);
		//Debug.Log ("fp: "+fp+" pp: "+pp+" sum: "+sum);
		return sum;
	}*/
	
	private bool intersects(Vector3 pos1,Vector3 scale1, Vector3 pos2, Vector3 scale2 ){
		Bounds b1 = new Bounds(pos1,scale1);
		Bounds b2 = new Bounds(pos2,scale2);
		if (b1.Intersects(b2)) return true;
		return false;
	}
	
	
}
