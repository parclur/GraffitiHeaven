using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiGoDown : MonoBehaviour {

    Transform diver;
    Transform drone;

    public float scaledDistance;

    [SerializeField] GameObject[] connections;

    [SerializeField] float[] distances;

    void Start(){
        diver = GameObject.FindGameObjectWithTag("Diver").transform;
        drone = GameObject.FindGameObjectWithTag("Drone").transform;
    }

    void Update(){
        GetScaledDistance();
        SetUIConnection();
    }

    void GetScaledDistance(){
        float tempDist = Vector3.Distance(diver.position, drone.position);
        scaledDistance = tempDist;
    }

    void SetUIConnection(){
        for(int i = 0; i < connections.Length; i++){
            if(scaledDistance <= distances[i]){
                connections[i].SetActive(true);
            }
            else {
                connections[i].SetActive(false);
            }
        }
    }
}
