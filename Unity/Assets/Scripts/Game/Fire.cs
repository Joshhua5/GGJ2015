using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{

    [SerializeField]
    private GameObject fireObject;

    [SerializeField]
    private float spawnRate;

	[SerializeField]
	private Room room;

    float frontDistance, backDistance, leftDistance, rightDistance;
    float charge;
    // Use this for initialization
    void Start()
    {
        charge = 0;
        // Get the direction to expand into 
        RaycastHit hit;

        int mask = (1 << Layers.Obstacles) | (1 << Layers.Ground);

        // Consider making 100 and then assume there's a large gap until a wall
        if (Physics.Raycast(new Ray(transform.position, Vector3.back), out hit, 1000, mask))
                backDistance = hit.distance;

        if (Physics.Raycast(new Ray(transform.position, Vector3.forward), out hit, 1000, mask))
                frontDistance = hit.distance;

        if (Physics.Raycast(new Ray(transform.position, Vector3.left), out hit, 1000, mask))
                leftDistance = hit.distance;

        if (Physics.Raycast(new Ray(transform.position, Vector3.right), out hit, 1000, mask))
                rightDistance = hit.distance;
        
    }

	public void setRoom(Room room){
		this.room = room;
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 modifier = new Vector3(0, 0, 0);
        charge += Time.deltaTime;
		if (room.HasWater()) Destroy(this.gameObject);
		else{
        // Possible to create fire nodes that don't spread to keep things in control
        if (charge > spawnRate)
        {
            charge = 0;
			if (!room.HasWater ()){
	            if (frontDistance > 32)
	            {
	                modifier = Vector3.forward;
	                frontDistance = 0;
	            }
	            else if (backDistance > 32)
	            {
	                modifier = Vector3.back;
	                backDistance = 0;
	            }
	            else if (leftDistance > 32)
	            {
	                modifier = Vector3.left;
	                leftDistance = 0;
	            }
	            else if (rightDistance > 32)
	            {
	                modifier = Vector3.right;
	                rightDistance = 0;
	            }
	             
	            RaycastHit hit;
	            
	            //Check that fire is still in the room
	            RaycastHit hitR;
	            Room cr = null;
				if (Physics.Raycast(new Ray(transform.position + (modifier * 16) + (Vector3.up * 20), Vector3.down), out hitR, 50, 1 << Layers.Ground))
				{
						// should collide with the floor prefab so get Room from parent component
						cr = hitR.collider.gameObject.GetComponentInParent<Room>();
				}
						
						// Consider making 100 and then assume there's a large gap until a wall
	            if (Physics.Raycast(new Ray(transform.position + (modifier * 16) + (Vector3.up * 20), Vector3.down), out hit, 50, 1 << Layers.Obstacles))
	                // Stop fire from expanding back into fire
	                if (hit.collider.gameObject.GetComponent<Fire>() != null)
	                {
	                    // If it failed to create an object here, don't try again
	                    if (modifier == Vector3.forward)
	                        frontDistance = 0;
	                    if (modifier == Vector3.back) 
	                        backDistance = 0;
	                    if (modifier == Vector3.left)
	                        leftDistance = 0;
	                    if (modifier == Vector3.right)
	                        rightDistance = 0;
	
	                    modifier.Set(0, 0, 0);
	
	                }    
				// check that we're in a room first and have it correctly hooked up
	            if (cr != null && !(modifier.x == 0 && modifier.y == 0 && modifier.z == 0))
	            { 
					//Debug.Log ("parent fire in "+room.ToString());
					//Debug.Log ("Spawn new fire in room "+cr.ToString());
	                // Spawn new fire
	                GameObject newFire = (GameObject)Instantiate(fireObject);
	                newFire.transform.parent = this.transform;
	                newFire.transform.localPosition = new Vector3(0, 0, 0);
	
	                newFire.transform.position += modifier * 16;
	                // Swap afterwards to be exluded from the raycast
	                newFire.gameObject.layer = Layers.Obstacles;
					Fire f = newFire.GetComponentInParent<Fire>();
					f.setRoom (cr);
				}
			}
        }
    }
}
}