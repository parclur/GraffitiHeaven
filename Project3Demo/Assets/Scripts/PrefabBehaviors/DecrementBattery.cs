using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecrementBattery : MonoBehaviour {
    
    [SerializeField] int batterySlots;

    void OnTriggerEnter(Collider col){
        UpdateActualBattery.instance.DecrementBattery(batterySlots);
    }
}
