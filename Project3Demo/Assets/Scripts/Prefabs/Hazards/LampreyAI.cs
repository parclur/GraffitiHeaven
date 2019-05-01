using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LampreyAI : MonoBehaviour {

    [SerializeField] HeavyDoor door;

    [SerializeField] float moveTime;

    [SerializeField] NavMeshAgent agent;

    public GameObject chaseObject;

    bool isLunging;

    bool findingPlayer;

    Vector3 currentPos;

    List<GameObject> waypoints;

    GameObject startWaypoint;

    void Start(){
        if(chaseObject == null){
            chaseObject = GameObject.FindGameObjectWithTag("Drone");
        }
        currentPos = transform.position;
        waypoints = new List<GameObject>();
    }
    
    void Update(){
        if(!door) Debug.Log(name + " Doesn't have a door!");
        //Only activate when the door is open
        if(door.doorOpen){
            float distanceToTarget = Vector3.Distance(agent.transform.position, chaseObject.transform.position);
            if(distanceToTarget < 1.5){
                SceneLoader.instance.LoseGame();
            }
            agent.destination = chaseObject.transform.position;
            AudioManager.instance.PlayOneShot("Monster1e", 1f);
            //If you can see the player, move to the player
            if(CanSeePlayer()){
                if(!isLunging){
                    //findingPlayer = false;
                    //agent.ResetPath();
                    StartCoroutine(JumpAtPlayer());
                }
            }
            //Else find a waypoint that can see the player
            else if(!findingPlayer){
                isLunging = false;
                findingPlayer = true;
                //waypoints.Clear();
                //FindWaypointMap();
                //StartCoroutine(MoveBetweenPoints());
            }
        }
    }

    IEnumerator MoveBetweenPoints(){
        waypoints.Reverse();
        waypoints.Add(chaseObject);
        for(int i = 0; i < waypoints.Count; i++){
            float elapsedTime = 0.0f;
            while (elapsedTime < moveTime){
                transform.position = Vector3.Lerp(currentPos, waypoints[i].transform.position, (elapsedTime / moveTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            currentPos = transform.position;
        }
    }

    void FindWaypointMap(){
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Waypoint");

        FindStartingPoint();

        foreach(GameObject go in gos){
            Vector3 dir = go.transform.position - (chaseObject.transform.position + transform.up);
            RaycastHit hit;

            int layerMask = 1 << 12;

            if(Physics.Raycast(chaseObject.transform.position + transform.up, dir, out hit, Mathf.Infinity, layerMask)){
                if(hit.transform.gameObject.tag == "Waypoint"){
                    //Find the waypoint that can see the player then work backwards. What waypoint can see this waypoint? What about that one?
                    waypoints.Add(hit.transform.gameObject);
                    if(FindWaypoint(0)){
                        return;
                    }
                    waypoints.Remove(hit.transform.gameObject);
                    //If that was not the correct waypoint check other waypoints
                }
            }
        }

        Debug.Log("ERROR! UNABLE TO FIND PATH");
    }

    void FindStartingPoint(){
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Waypoint");

        int layerMask = 1 << 12;

        //First find the starting point
        foreach(GameObject go in gos){
            Vector3 dir = go.transform.position - transform.position;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask)){
                if(hit.transform.gameObject.tag == "Waypoint"){
                    startWaypoint = hit.transform.gameObject;
                    return;
                }
            }
        }
    }

    bool FindWaypoint(int index){
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach(GameObject go in gos){
            Vector3 dir = go.transform.position - waypoints[index].transform.position;
            RaycastHit hit;

            int layerMask = 1 << 12;

            if(Physics.Raycast(waypoints[index].transform.position, dir, out hit, Mathf.Infinity, layerMask)){
                if(hit.transform.gameObject.tag == "Waypoint"){
                    if(hit.transform.gameObject == startWaypoint){
                        waypoints.Add(hit.transform.gameObject);
                        //Found the start, return all the way up
                        return true;
                    }
                    if(!waypoints.Contains(hit.transform.gameObject)){
                        waypoints.Add(hit.transform.gameObject);
                        if(FindWaypoint(index + 1)){
                            return true;
                        }
                        waypoints.Remove(hit.transform.gameObject);
                        //If that was not the correct waypoint check other waypoints
                    }
                }
            }
        }
        return false;
    }

    IEnumerator JumpAtPlayer(){
        AudioManager.instance.PlayOneShot("Monster2e", 1f);
        isLunging = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < moveTime){
            gameObject.transform.position = Vector3.Lerp(currentPos, chaseObject.transform.position, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SceneLoader.instance.LoseGame();
    }

    bool CanSeePlayer(){
        Vector3 dir = chaseObject.transform.position - transform.position;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity)){
            if(hit.transform.tag == "Drone"){
                return true;
            }
        }
        return false;
    }
}