using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFishDetect : MonoBehaviour {
    void OnTriggerEnter(Collider col){
        if(col.tag == "Diver"){
            GetComponentInParent<AnglerFish>().CheckSeePlayer();
        }
    }

    void OnTriggerStay(Collider col){
        if(col.tag == "Diver"){
            GetComponentInParent<AnglerFish>().CheckSeePlayer();
        }
    }
}
