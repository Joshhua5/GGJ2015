using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private GameObject fireObject;

    [SerializeField]
    private float spawnRate;

    float frontDistance, backDistance, leftDistance, rightDistance;
    float charge;
    // Use this for initialization
    void Start()
    {
        charge = 0;
        // Get the direction to expand into 
        RaycastHit hit;

        // Consider making 100 and then assume there's a large gap until a wall
        if (Physics.Raycast(new Ray(transform.position, Vector3.back), out hit, 1000, 1 << Layers.Obstacles))
        {
            // Stop fire from expanding back into fire
            if (hit.collider.gameObject.GetComponent<Fire>() != null)
                frontDistance = 0;
            else
                frontDistance = hit.distance;
        }
        if (Physics.Raycast(new Ray(transform.position, Vector3.forward), out hit, 1000, 1 << Layers.Obstacles))
        {
            if (hit.collider.gameObject.GetComponent<Fire>() != null)
                backDistance = 0;
            else
                backDistance = hit.distance;
        }
        if (Physics.Raycast(new Ray(transform.position, Vector3.left), out hit, 1000, 1 << Layers.Obstacles))
        {
            if (hit.collider.gameObject.GetComponent<Fire>() != null)
                leftDistance = 0;
            else
                leftDistance = hit.distance;
        }
        if (Physics.Raycast(new Ray(transform.position, Vector3.right), out hit, 1000, 1 << Layers.Obstacles))
        {
            if (hit.collider.gameObject.GetComponent<Fire>() != null)
                rightDistance = 0;
            else
                rightDistance = hit.distance;
        }
    }



    // Update is called once per frame
    void Update()
    {
        charge += Time.deltaTime;
        // Possible to create fire nodes that don't spread to keep things in control
        if (charge > spawnRate)
        {  
            charge = 0;
            // Spawn new fire
            GameObject newFire = (GameObject)Instantiate(fireObject);
            newFire.transform.parent = this.transform;
            newFire.transform.localPosition = new Vector3(0, 0, 0);

            Vector3 modifier = new Vector3(0, 0, 0);

            if (frontDistance > 32)
                modifier = Vector3.forward;
            else if (backDistance > 32)
                modifier = Vector3.back;
            else if (leftDistance > 32)
                modifier = Vector3.left;
            else if (rightDistance > 32)
                modifier = Vector3.right;

            newFire.transform.position += modifier * 16;

            // destroy if the object wasn't able to move.
            if (modifier.x == 0 && modifier.y == 0 && modifier.z == 0)
                DestroyObject(newFire);
        }
    }
}
