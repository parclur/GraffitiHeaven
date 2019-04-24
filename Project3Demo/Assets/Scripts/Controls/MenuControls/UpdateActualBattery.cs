using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateActualBattery : MonoBehaviour {

    [HideInInspector] public static UpdateActualBattery instance;
    [SerializeField] GameObject[] batteryList;

    int batteryIndex;

    void Start(){
        instance = this;
        batteryIndex = batteryList.Length - 1;
    }

    public void DecrementBattery(){
        batteryIndex--;
        for(int i = 0; i < batteryList.Length; i++){
            if(batteryIndex < i){
                batteryList[i].SetActive(false);
            }
        }
    }

    public void DecrementBattery(int num){
        batteryIndex = num;
        for(int i = 0; i < batteryList.Length; i++){
            if(batteryIndex < i){
                batteryList[i].SetActive(false);
            }
        }
    }

    public void IncrementBattery(){
        batteryIndex++;
        for(int i = 0; i < batteryList.Length; i++){
            if(batteryIndex >= i){
                batteryList[i].SetActive(true);
            }
        }
    }
}
