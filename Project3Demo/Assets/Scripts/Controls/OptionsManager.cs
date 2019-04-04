using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {

    [HideInInspector] public static OptionsManager instance;
    
    public enum SettingsName {Sound, brightness, grain, vignette, ambientOcclusion, ArraySize};

    public float[] settingsLevel;

    void Start(){
        settingsLevel = new float[(int)SettingsName.ArraySize];
        for (int i = 0; i < settingsLevel.Length; i++){
            settingsLevel[i] = 100;
        }
        instance = this;
    }

    void Update(){
        for(int i = 0; i < settingsLevel.Length; i++){
            if (settingsLevel[i] < 0){
                settingsLevel[i] = 0;
            }
            if (settingsLevel[i] > 100){
                settingsLevel[i] = 100;
            }
        }

        if(PostProcessingManager.instance){
            //TODO: Change this out once the function is completed
            PostProcessingManager.instance.AdjustSetting(settingsLevel[(int)SettingsName.grain] / 100, settingsLevel[(int)SettingsName.vignette] / 100, settingsLevel[(int)SettingsName.ambientOcclusion] / 100);  
        }
    }
}
