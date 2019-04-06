using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDoorWithLight : MonoBehaviour {

    private float angleToMatch;

    GameObject[] doors;

    void Start(){
        angleToMatch = GetComponent<Light>().spotAngle;
        doors = GameObject.FindGameObjectsWithTag("Door");
    }
    
    void Update(){
        CheckDoorHit();
    }

    void CheckDoorHit(){
        Vector3 position = transform.position;

        foreach (GameObject go in doors){
            Vector3 targetDir = go.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up) - 30f;
            if(Mathf.Abs(angle) < angleToMatch){
                go.GetComponent<HeavyDoor>().stopOpening = true;
            }
            else {
                go.GetComponent<HeavyDoor>().stopOpening = false;
            }
        }
    }
}
