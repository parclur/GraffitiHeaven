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
        timerText.text = Mathf.FloorToInt(currentTime / 60).ToString() + ":" + Mathf.FloorToInt(currentTime % 60).ToString();
    }
}
