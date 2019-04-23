using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDoorWithLight : MonoBehaviour {

    private float angleToMatch;

    GameObject[] doors;
    HeavyDoor[] hDoors;

    void Start(){
        angleToMatch = GetComponent<Light>().spotAngle;
        doors = GameObject.FindGameObjectsWithTag("Door");
        hDoors = new HeavyDoor[doors.Length];
        for(int i = 0; i < hDoors.Length; i++){
            hDoors[i] = doors[i].GetComponent<HeavyDoor>();
        }
    }
    
    void Update(){
        CheckDoorHit();
    }

    void CheckDoorHit(){
        Vector3 position = transform.position;

        for(int i = 0; i < doors.Length; i++){
            GameObject go = doors[i];
            Vector3 targetDir = go.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up) - 30f;
            if(Mathf.Abs(angle) < angleToMatch){
                if(hDoors[i]){
                    hDoors[i].stopOpening = true;
                }
            }
            else {
                if(hDoors[i]){
                    hDoors[i].stopOpening = true;
                }
            }
        }
    }
}
