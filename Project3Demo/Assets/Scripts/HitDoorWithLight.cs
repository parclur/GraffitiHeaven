using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDoorWithLight : MonoBehaviour {

    private float angleToMatch;

    void Start(){
        angleToMatch = GetComponent<Light>().spotAngle;
    }
    
    void Update(){
        CheckDoorHit();
    }

    void CheckDoorHit(){
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Door");
        Vector3 position = transform.position;

        foreach (GameObject go in gos){
            Vector3 targetDir = go.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up) - 30f;
            Debug.Log("Angle: " + angle);
            Debug.Log("Angle to Match: " + angleToMatch);
            if(Mathf.Abs(angle) < angleToMatch){
                go.GetComponent<HeavyDoor>().stopOpening = true;
            }
            else {
                go.GetComponent<HeavyDoor>().stopOpening = false;
            }
        }
    }
}
