using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFish : MonoBehaviour {

    public bool playerDetected;

    [SerializeField] float chaseCooldown;
    
    [SerializeField] float moveTime;
    
    //These are different from lampreywaypoints in which they are not tagged
    [SerializeField] List<GameObject> patrolPoint;

    Vector3 currentPos;

    bool movingFinished = true;

    bool chaseFinished = true;

    GameObject diver;

    int pointIndex = 0;

    void Start(){
        currentPos = transform.position;
        diver = GameObject.FindGameObjectWithTag("Diver");
    }
    
    void Update(){
        if(playerDetected){
            //Chase after player
            StartCoroutine(ChaseTimer());
            if(chaseFinished){
                StopAllCoroutines();
                chaseFinished = false;
                StartCoroutine(ChaseAtPlayer());
            }
        }
        else if(movingFinished){
            //Pursue the waypoints
            StopAllCoroutines();
            movingFinished = false;
            StartCoroutine(MoveBetweenPoints());
        }
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
                transform.position = Vector3.Lerp(currentPos, patrolPoint[i].transform.position, (elapsedTime / moveTime));
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
        float elapsedTime = 0.0f;
        while(elapsedTime < moveTime){
            if(!playerDetected){
                yield return null;
            }
            transform.LookAt(diver.transform);
            transform.position = Vector3.Lerp(currentPos, diver.transform.position + (diver.transform.up * 2), (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        currentPos = transform.position;
        chaseFinished = true;
    }

    IEnumerator ChaseTimer(){
        yield return new WaitForSeconds(chaseCooldown);
        playerDetected = false;
    }
}
