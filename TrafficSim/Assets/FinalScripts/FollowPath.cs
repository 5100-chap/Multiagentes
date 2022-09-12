using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    // Target
    public Transform goal;
    // Speed
    [Range(0f, 150f)]
    public float speed = 5.0f;
    // Final distance from target
    float accuracy = 1.0f;
    // Rotation speed
    public float rotSpeed = 2.0f;
    // Access to the WPManager script
    public GameObject wpManager;
    // Array of waypoints
    public GameObject[] wps;
    // Current waypoint
    [SerializeField]
    public GameObject currentNode;
    // Starting waypoint index
    int currentWP = 0;
    // Access to the Graph script
    [SerializeField]
    public Graph g = new Graph();
    
    public bool Activate;
    public GameObject raycastStartingPoint;
    public float collisionRaycastLength = 7f;
    public float collisionRaycastEmergencyLength = 2f;
    public int collisions;

    private int randomWaypoint;
    private bool stop;
    private bool collisionStop = false;
    private bool emergency = false;
    

    // Use this for initialization
    void Start() {

        // Get hold of wpManager and Graph scripts
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        collisions = 0;
        Activate = true;
    }

    // Instantiates a random Waypoint in the map and its corresponding path
    public void MakePath() {

        randomWaypoint = Random.Range(0, 199);
        currentNode = ClosestWaypoint(transform.position);
        // Use the AStar method passing it currentNode and destination
        g.AStar(currentNode, wps[randomWaypoint]);
        // Reset index
        currentWP = 0;
    }

    //Verify if the car raycast collides within certain distance with other car
    private void CheckForCollisions()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastStartingPoint.transform.position, transform.forward, out hit, collisionRaycastLength, 1 << gameObject.layer))
        {
            if (hit.transform.tag == "Car")
            {
                collisionStop = true;
            }
        }
        else
        {
            collisionStop = false;
        }
    }

    //Verify if the car raycast collides within certain smaller distance with other car
    private void CheckForCollisionsEmergency()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastStartingPoint.transform.position, transform.forward, out hit, collisionRaycastEmergencyLength, 1 << gameObject.layer))
        {
            if (hit.transform.tag == "Car")
            {
                emergency = true;
            }
        }
        else
        {
            emergency = false;
        }
    }


    // Registers the amount of collisions of the car
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Car")
        {
            collisions = collisions + 1;
        }
    }

    // Draws the raycast of collision
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Vector3 dir = transform.TransformDirection(Vector3.forward) * collisionRaycastLength;
    //    Gizmos.DrawRay(raycastStartingPoint.transform.position, dir);
    //}

    // Decelerate
    public bool Break
    {
        get { return stop || collisionStop; }
        set { stop = value; }
    }

    // ABS Break
    public bool EmergencyBreak
    {
        get { return stop || collisionStop; }
        set { stop = value; }
    }

    // Returns the closest waypoint relative to the actual position
    public GameObject ClosestWaypoint(Vector3 pos)
    {
        float dist = Mathf.Infinity;
        GameObject waypoint = new GameObject();
        foreach(GameObject obj in wps)
        {
            if(dist > (obj.transform.position - pos).sqrMagnitude)
            {
                dist = (obj.transform.position - pos).sqrMagnitude;
                waypoint = obj;
            }
        }
        return waypoint;
    }

    // Update is called once per frame
    void LateUpdate() {
        
        if(Activate)
        {
            MakePath();
            Activate = false;
        }
        CheckForCollisions();
        CheckForCollisionsEmergency();
        if(Break)
        {
            speed = speed - 5f;
            rotSpeed = speed / 2;
        }
        if(!Break)
        {
            speed = speed + 0.03f;
            rotSpeed = speed / 2;
        }
        if(EmergencyBreak)
        {
            speed = 0.1f;
        }

        // If the car nowhere to go then just return
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
        {
            Debug.Log(g.getPathLength());
            GameObject.Destroy(this.gameObject);
            return;
        }
            

        //the node we are closest to at this moment
        currentNode = g.getPathPoint(currentWP);

        //if the car is close enough to the current waypoint move to next
        if (Vector3.Distance(
            g.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy) {
            currentWP++;
        }

        //if the car is not at the end of the path
        if (currentWP < g.getPathLength()) {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                                            goal.position.y,
                                            goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            if((lookAtGoal.y - transform.position.y) > 0.2f || (lookAtGoal.y - transform.position.y) < -0.2f)
            {
                speed = 20f;
            }
            if (((lookAtGoal.x - transform.position.x) > 0.5f || (lookAtGoal.x - transform.position.x) < -0.5f) && ((lookAtGoal.z - transform.position.z) > 0.5f || (lookAtGoal.z - transform.position.z) < -0.5f))
            {
                speed = 20f;
            }

                // Rotate towards the heading
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * rotSpeed);

            // Move the car
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            speed = 0f;
        }

    }
}