using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorFlare : MonoBehaviour {

    void OnTriggerEnter(Collider col){
        if(col.tag == "Flare") GetComponentInChildren<HeavyDoor>().TriggerFlareOnDoor();
    }
}
