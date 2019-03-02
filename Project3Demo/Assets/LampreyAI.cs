using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampreyAI : MonoBehaviour {

    [SerializeField] HeavyDoor door;

    [SerializeField] float moveTime;

    public GameObject diver;

    bool isLunging;

    Vector3 currentPos;

    void Start(){
        diver = GameObject.FindGameObjectWithTag("Diver");
        currentPos = transform.position;
    }
    
    void Update(){
        //Only activate when the door is open
        if(door.doorOpen){
            //If you can see the player, move to the player
            if(CanSeePlayer()){
                if(!isLunging){
                    StartCoroutine(JumpAtPlayer());
                }
            }
            //Else find a waypoint that can see the player
            else {
                MoveToPlayerWaypoint();
            }
        }
    }

    void MoveToPlayerWaypoint(){
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject go in gos){
            Vector3 dir = go.transform.position - transform.position;
            RaycastHit[] sight = Physics.RaycastAll(transform.position, dir);
            foreach(RaycastHit hit in sight){
                if(hit.transform.gameObject.tag == "Waypoint"){
                    //Find the waypoint that can see the player then work backwards. What waypoint can see this waypoint? What about that one?
                }
            }
        }
    }

    IEnumerator JumpAtPlayer(){
        isLunging = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < moveTime){
            gameObject.transform.position = Vector3.Lerp(currentPos, diver.transform.position, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    bool CanSeePlayer(){
        Vector3 dir = diver.transform.position - transform.position;
        RaycastHit[] sight = Physics.RaycastAll(transform.position, dir);
        foreach(RaycastHit hit in sight){
            if(hit.transform.gameObject.tag == "Diver"){
                return true;
            }
		}
        return false;
    }
}
