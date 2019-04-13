using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCurrentTime : MonoBehaviour {
    
    float currentTime = 0;
    Text timerText;
    
    void Start(){
        timerText = GetComponent<Text>();
    }
    
    void Update(){
        currentTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(currentTime % 60);
        if(seconds < 10){
            timerText.text = Mathf.FloorToInt(currentTime / 60).ToString() + ":0" + seconds.ToString();
        }
        else {
            timerText.text = Mathf.FloorToInt(currentTime / 60).ToString() + ":" + seconds.ToString();
        }
    }
}
