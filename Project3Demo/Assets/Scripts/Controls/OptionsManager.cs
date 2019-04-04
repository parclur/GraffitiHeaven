using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {

    [HideInInspector] public static OptionsManager instance;
    
    public float soundVolume = 100;
    public float postProcessingLevel = 100;
    public float brightnessLevel = 100;

    void Start(){
        instance = this;
    }

    void Update(){
        if(soundVolume < 0){
            soundVolume = 0;
        }
        if(soundVolume > 100) {
            soundVolume = 100;
        }
        if(postProcessingLevel < 0){
            postProcessingLevel = 0;
        }
        if(postProcessingLevel > 100){
            postProcessingLevel = 100;
        }
        if(brightnessLevel < 0){
            brightnessLevel = 0;
        }
        if(brightnessLevel > 100){
            brightnessLevel = 100;
        }
    }
}
