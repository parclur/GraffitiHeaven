using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFish : MonoBehaviour {

    public bool playerDetected;
    public bool collidedWithPlayer;

    [SerializeField] float chaseCooldown;
    
    [SerializeField] float moveTime;

    [SerializeField] float stunTime;

    [SerializeField] float windTime;

    [SerializeField] float chaseTime;
    
    //These are different from lampreywaypoints in which they are not tagged
    [SerializeField] List<GameObject> patrolPoint;

    Vector3 currentPos;

    bool movingFinished = true;
    bool chaseFinished = true;
    bool stunned;
    bool chasing;

    GameObject diver;

    GameObject anglerLight;

    int pointIndex = 0;

    Vector3 prevDiverPos;

    void Start(){
        currentPos = transform.position;
        diver = GameObject.FindGameObjectWithTag("Diver");
        anglerLight = transform.Find("Spot Light").gameObject;
    }
    
    void Update(){
        if(Vector3.Distance(diver.transform.position + (diver.transform.up * 2), this.transform.position) <= .6){
            collidedWithPlayer = true;
        }

        if(collidedWithPlayer){
            ResetCoroutines();
            collidedWithPlayer = false;

            diver.GetComponent<PlayerDiverMovement>().ApplySlow();
        }

        if(stunned){
            return;
        }
        
        else if(playerDetected){
            //Chase after player
            if(chaseFinished){
                ResetCoroutines();
                chaseFinished = false;
                playerDetected = false;
                //StartCoroutine(LookAround());
            }
            else if(!chasing){
                chasing = true;
                StartCoroutine(ChaseAtPlayer());
            }
        }
        else if(movingFinished){
            //Pursue the waypoints
            ResetCoroutines();
            movingFinished = false;
            StartCoroutine(MoveBetweenPoints());
        }
        if(playerDetected){
            CheckSeePlayer();
        }
    }

    void ResetCoroutines(){
        StopAllCoroutines();
        chaseFinished = true;
        movingFinished = true;
        chasing = false;
    }

    public void CheckSeePlayer(){
        RaycastHit hit;
        Vector3 dir = diver.transform.position - transform.position;
        if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity)){
            if(hit.transform.tag == "Diver"){
                Debug.DrawRay(transform.position, dir);
                playerDetected = true;
                AudioManager.instance.PlayOneShot("SeaMonster1", 1f);       
                return;
            }
        }
        playerDetected = false;
    }

    IEnumerator MoveBetweenPoints(){
        currentPos = transform.position;
        for(int i = pointIndex; i < patrolPoint.Count; i++){
            pointIndex = i;
            float elapsedTime = 0.0f;
            while(elapsedTime < moveTime){
                if(playerDetected){
                    yield return null;
                }
                transform.LookAt(patrolPoint[i].transform);
                transform.position = Vector3.Lerp(currentPos,patrolPoint[i].transform.position,(elapsedTime / moveTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            currentPos = transform.position;
        }
        pointIndex = 0;
        movingFinished = true;
    }

    IEnumerator ChaseAtPlayer(){
        currentPos = transform.position;
        prevDiverPos = diver.transform.position;
        float elapsedTime = 0.0f;
        while(elapsedTime < chaseTime){
            if(!playerDetected){
                yield return null;
            }
            //From the unity docs. Changes rotation of y and nothing else
            Vector3 relativePos = prevDiverPos - transform.position;
            
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = new Quaternion(transform.rotation.x, rotation.y, transform.rotation.z, transform.rotation.w);

            transform.position = Vector3.Lerp(currentPos, prevDiverPos + (Vector3.up * 2), (elapsedTime / chaseTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        currentPos = transform.position;
        chaseFinished = true;
        chasing = false;
    }

    IEnumerator ChaseTimer(){
        yield return new WaitForSeconds(chaseCooldown);
        playerDetected = false;
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Flare"){
            stunned = true;
            StopAllCoroutines();
            StartCoroutine(Stun());
        }
    }

    IEnumerator Stun(){
        anglerLight.SetActive(false);
        yield return new WaitForSeconds(stunTime);
        anglerLight.SetActive(true);
        stunned = false;
    }

    //Repurposed from the unity docs https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html. Find closest waypoint
    public int FindClosestWaypoint(){
        int num = -1;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        for(int i = 0; i < patrolPoint.Count; i++){
            Vector3 diff = patrolPoint[i].transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance){
                num = i;
                distance = curDistance;
            }
        }
        return num;
    }
}
